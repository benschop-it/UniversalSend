using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Common;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Helpers;
using UniversalSend.Services.Interfaces.Internal;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace UniversalSend.Services.Misc {

    internal class OperationFunctions : IOperationFunctions {

        #region Private Fields

        private readonly ILogger _logger;
        private readonly IDeviceManager _deviceManager;
        private readonly IHistoryManager _historyManager;
        private readonly IReceiveManager _receiveManager;
        private readonly IReceiveTaskManager _receiveTaskManager;
        private readonly IRegister _register;

        #endregion Private Fields

        #region Public Constructors

        public OperationFunctions(
            IReceiveManager receiveManager,
            IReceiveTaskManager receiveTaskManager,
            IRegister register,
            IDeviceManager deviceManager,
            IHistoryManager historyManager
        ) {
            _logger = LogManager.GetLogger<OperationFunctions>();
            _receiveManager = receiveManager ?? throw new ArgumentNullException(nameof(receiveManager));
            _receiveTaskManager = receiveTaskManager ?? throw new ArgumentNullException(nameof(receiveTaskManager));
            _register = register ?? throw new ArgumentNullException(nameof(register));
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
            _historyManager = historyManager ?? throw new ArgumentNullException(nameof(historyManager));
        }

        #endregion Public Constructors

        #region Public Methods

        public object LegacyInfoRequestFuncV1(IMutableHttpServerRequest mutableHttpServerRequest) {
            _logger.Debug($"LegacyInfoRequestFuncV1: remote={mutableHttpServerRequest?.RemoteAddress}, uri={mutableHttpServerRequest?.Uri}");
            _ = TryRegisterLegacyCallerAsync(mutableHttpServerRequest);
            return null;
        }

        // Handles registration request from remote device
        public object RegisterRequestFuncV2(IMutableHttpServerRequest mutableHttpServerRequest) {
            if (string.IsNullOrWhiteSpace(mutableHttpServerRequest.RemoteAddress)) {
                _logger.Debug("RegisterRequestFuncV2: missing remote address.");
                return null;
            }

            string jsonStr = StringHelper.ByteArrayToString(mutableHttpServerRequest.Content);

            _logger.Debug($"RegisterRequestFunc: {mutableHttpServerRequest.RemoteAddress} {jsonStr}.");

            IRegisterRequestDataV2 registerRequestData = JsonConvert.DeserializeObject<RegisterRequestDataV2>(jsonStr);
            if (registerRequestData == null) {
                return null;
            }

            if (registerRequestData.Port <= 0) {
                _logger.Debug($"RegisterRequestFuncV2: invalid remote port '{registerRequestData.Port}'.");
                return null;
            }

            IDevice device = _deviceManager.GetDeviceFromRequestDataV2(
                registerRequestData,
                mutableHttpServerRequest.RemoteAddress,
                registerRequestData.Port
            );
            _register.NewDeviceRegisterEvent(device);
            return null;
        }

        // Handles incoming file from remote device
        public async Task<object> SendRequestFuncAsync(IMutableHttpServerRequest mutableHttpServerRequest) {

            Dictionary<string, string> queryParameters = StringHelper.GetURLQueryParameters(mutableHttpServerRequest.Uri.ToString());
            _logger.Debug($"SendRequestFuncAsync: Received file. URI: {mutableHttpServerRequest.Uri.ToString()}, Number of query parameters: {queryParameters.Count}.");

            string sessionId, fileId, token;
            if (
                !queryParameters.TryGetValue("sessionId", out sessionId) ||
                !queryParameters.TryGetValue("fileId", out fileId) || 
                !queryParameters.TryGetValue("token", out token)
            ) {
                _receiveManager.SendDataReceivedEvent(null);
                return null;
            }

            IReceiveTask task = await _receiveTaskManager.WriteFileContentToReceivingTaskV2(sessionId, fileId, token, mutableHttpServerRequest.Content);

            if (task != null) {
                _receiveManager.SendDataReceivedEvent(task);
                _logger.Debug("Writing data to file.");
                var headerList = mutableHttpServerRequest.Headers.ToList();
                var item = headerList.Find(x => x.Name.Equals("host"));
                if (item == null) {
                    return null;
                }

                string host = item.Value;
                string ip = host.Substring(0, host.LastIndexOf(":"));
                await WriteFileAsync(task, ip);
            } else {
                _receiveManager.SendDataReceivedEvent(null);
            }

            return null;
        }

        // Handles incoming file from remote device
        public async Task<object> UploadRequestFuncAsync(IMutableHttpServerRequest mutableHttpServerRequest) {

            Dictionary<string, string> queryParameters = StringHelper.GetURLQueryParameters(mutableHttpServerRequest.Uri.ToString());
            _logger.Debug($"SendRequestFuncAsync: Received file. URI: {mutableHttpServerRequest.Uri.ToString()}, Number of query parameters: {queryParameters.Count}.");

            string sessionId, fileId, token;
            if (
                !queryParameters.TryGetValue("sessionId", out sessionId) ||
                !queryParameters.TryGetValue("fileId", out fileId) ||
                !queryParameters.TryGetValue("token", out token)
            ) {
                _receiveManager.SendDataReceivedEvent(null);
                return null;
            }

            IReceiveTask task = await _receiveTaskManager.WriteFileContentToReceivingTaskV2(sessionId, fileId, token, mutableHttpServerRequest.Content);

            if (task != null) {
                _receiveManager.SendDataReceivedEvent(task);
                _logger.Debug("Writing data to file.");
                var headerList = mutableHttpServerRequest.Headers.ToList();
                var item = headerList.Find(x => x.Name.Equals("host"));
                if (item == null) {
                    return null;
                }

                string host = item.Value;
                string ip = host.Substring(0, host.LastIndexOf(":"));
                await WriteFileAsync(task, ip);
            } else {
                _receiveManager.SendDataReceivedEvent(null);
            }

            return null;
        }


        #endregion Public Methods

        #region Private Methods

        private async Task TryRegisterLegacyCallerAsync(IMutableHttpServerRequest request) {
            if (request == null || string.IsNullOrWhiteSpace(request.RemoteAddress)) {
                _logger.Debug("TryRegisterLegacyCallerAsync: missing request or remote address.");
                return;
            }

            for (int attempt = 1; attempt <= 3; attempt++) {
                try {
                    if (attempt > 1) {
                        await Task.Delay(attempt * 500);
                    }

                    _logger.Debug($"TryRegisterLegacyCallerAsync: attempt {attempt}, probing caller via FindDeviceByIPAsync for '{request.RemoteAddress}'.");

                    IDevice device = await _deviceManager.FindDeviceByIPAsync(request.RemoteAddress);
                    if (device == null) {
                        _logger.Debug("TryRegisterLegacyCallerAsync: FindDeviceByIPAsync returned null.");
                        continue;
                    }

                    if (device.Fingerprint == ProgramData.LocalDevice.Fingerprint) {
                        _logger.Debug("TryRegisterLegacyCallerAsync: discovered device matched local fingerprint, skipping add.");
                        return;
                    }

                    _logger.Debug($"TryRegisterLegacyCallerAsync: adding device alias='{device.Alias}', ip='{device.IP}', port={device.Port}, fingerprint='{device.Fingerprint}'.");
                    _deviceManager.AddKnownDevices(device);
                    return;
                } catch (Exception ex) {
                    _logger.Debug($"TryRegisterLegacyCallerAsync attempt {attempt} failed: {ex}");
                }
            }

            _logger.Debug("TryRegisterLegacyCallerAsync: all attempts failed.");
        }

        private async Task WriteFileAsync(IReceiveTask task, string ip) {
            IStorageFile file = null;
            try {
                file = await _receiveTaskManager.WriteReceiveTaskToFileV2Async(task);
            } catch (Exception ex) {
                _logger.Error($"Exception trying to write file {ex.Message}");
            }

            if (file != null) {
                var history = _historyManager.CreateHistory(
                    task.FileV2,
                    StorageApplicationPermissions.FutureAccessList.Add(file),
                    //_deviceManager.CreateDeviceFromInfoDataV1(task.SenderV1)
                    _deviceManager.CreateDeviceFromInfoDataV2(task.SenderV2)
                );

                _historyManager.AddHistoriesList(history);
            }
        }

        #endregion Private Methods

    }
}