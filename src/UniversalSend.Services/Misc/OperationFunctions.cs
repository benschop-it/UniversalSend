using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Helpers;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.Interfaces.Internal;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace UniversalSend.Services.Misc {

    internal class OperationFunctions : IOperationFunctions {

        #region Private Fields

        private IDeviceManager _deviceManager;
        private IHistoryManager _historyManager;
        private IReceiveManager _receiveManager;
        private IReceiveTaskManager _receiveTaskManager;
        private IRegister _register;

        #endregion Private Fields

        #region Public Constructors

        public OperationFunctions(
            IReceiveManager receiveManager,
            IReceiveTaskManager receiveTaskManager,
            IRegister register,
            IDeviceManager deviceManager,
            IHistoryManager historyManager
        ) {
            _receiveManager = receiveManager ?? throw new ArgumentNullException(nameof(receiveManager));
            _receiveTaskManager = receiveTaskManager ?? throw new ArgumentNullException(nameof(receiveTaskManager));
            _register = register ?? throw new ArgumentNullException(nameof(register));
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
            _historyManager = historyManager ?? throw new ArgumentNullException(nameof(historyManager));
        }

        #endregion Public Constructors

        #region Public Methods

        // Handles registration request from remote device
        public object RegisterRequestFunc(IMutableHttpServerRequest mutableHttpServerRequest) {
            var headerList = ((MutableHttpServerRequest)mutableHttpServerRequest).Headers.ToList();
            var item = headerList.Find(x => x.Name.Equals("host"));
            if (item == null) {
                return null;
            }

            string host = item.Value;
            string ip = host.Substring(0, host.LastIndexOf(":"));
            string portStr = host.Substring(host.LastIndexOf(":") + 1);
            int port = Convert.ToInt32(portStr);

            string jsonStr = StringHelper.ByteArrayToString(mutableHttpServerRequest.Content);

            Debug.WriteLine($"RegisterRequestFunc: {host} {ip} {portStr} {jsonStr}");

            IRegisterRequestData registerRequestData = JsonConvert.DeserializeObject<RegisterRequestData>(jsonStr);
            if (registerRequestData == null) {
                return null;
            }

            IDevice device = _deviceManager.GetDeviceFromRequestData(registerRequestData, ip, port);
            _register.NewDeviceRegisterV1Event(device);
            return null;
        }

        // Handles incoming file from remote device
        public async Task<object> SendRequestFuncAsync(IMutableHttpServerRequest mutableHttpServerRequest) {
            Debug.WriteLine($"SendRequestFuncAsync {mutableHttpServerRequest.Uri.ToString()}");

            Dictionary<string, string> queryParameters = StringHelper.GetURLQueryParameters(mutableHttpServerRequest.Uri.ToString());
            Debug.WriteLine($"Received file. Number of query parameters: {queryParameters.Count}");

            string fileId, token;
            if (!queryParameters.TryGetValue("fileId", out fileId) || !queryParameters.TryGetValue("token", out token)) {
                _receiveManager.SendDataReceivedEvent(null);
                return null;
            }

            IReceiveTask task = _receiveTaskManager.WriteFileContentToReceivingTask(fileId, token, mutableHttpServerRequest.Content);

            if (task != null) {
                _receiveManager.SendDataReceivedEvent(task);
                Debug.WriteLine("Writing data to file");
                var headerList = ((MutableHttpServerRequest)mutableHttpServerRequest).Headers.ToList();
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

        private async Task WriteFileAsync(IReceiveTask task, string ip) {
            IStorageFile file = await _receiveTaskManager.WriteReceiveTaskToFileAsync(task);
            if (file != null) {
                var history = _historyManager.CreateHistory(
                    task.File,
                    StorageApplicationPermissions.FutureAccessList.Add(file),
                    _deviceManager.CreateDeviceFromInfoData(task.Sender)
                );

                _historyManager.AddHistoriesList(history);
            }
        }

        #endregion Private Methods

    }
}