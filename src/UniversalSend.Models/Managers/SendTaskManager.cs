using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UniversalSend.Models.Common;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Interfaces.Internal;
using UniversalSend.Models.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Managers {

    internal class SendTaskManager : ISendTaskManager {

        #region Private Fields

        private readonly ILogger _logger;
        private IFileRequestDataManager _fileRequestDataManager;
        private IHttpClientHelper _httpClientHelper;
        private IInfoDataManager _infoDataManager;
        private INetworkHelper _networkHelper;
        private ISendManager _sendManager;
        private ISendTaskV2 _sendTask;
        private ISettings _settings;
        private IStorageHelper _storageHelper;
        private IUniversalSendFileManager _universalSendFileManager;
        private IWebSendManager _webSendManager;

        #endregion Private Fields

        #region Public Constructors

        public SendTaskManager(
            IUniversalSendFileManager universalSendFileManager,
            IInfoDataManager infoDataManager,
            INetworkHelper networkHelper,
            IStorageHelper storageHelper,
            ISendTaskV2 sendTask,
            ISendManager sendManager,
            IHttpClientHelper httpClientHelper,
            IFileRequestDataManager fileRequestDataManager,
            IWebSendManager webSendManager,
            ISettings settings
        ) {
            _logger = LogManager.GetLogger<SendTaskManager>();
            _universalSendFileManager = universalSendFileManager ?? throw new System.ArgumentNullException(nameof(universalSendFileManager));
            _infoDataManager = infoDataManager ?? throw new System.ArgumentNullException(nameof(infoDataManager));
            _networkHelper = networkHelper ?? throw new ArgumentNullException(nameof(networkHelper));
            _storageHelper = storageHelper ?? throw new System.ArgumentNullException(nameof(storageHelper));
            _sendTask = sendTask ?? throw new System.ArgumentNullException(nameof(sendTask));
            _sendManager = sendManager ?? throw new System.ArgumentNullException(nameof(sendManager));
            _httpClientHelper = httpClientHelper ?? throw new ArgumentNullException(nameof(httpClientHelper));
            _fileRequestDataManager = fileRequestDataManager ?? throw new ArgumentNullException(nameof(fileRequestDataManager));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _webSendManager = webSendManager ?? throw new ArgumentNullException(nameof(webSendManager));
        }

        #endregion Public Constructors

        #region Public Properties

        public string LastPrepareUploadErrorMessage { get; private set; }

        public int LastPrepareUploadStatusCode { get; private set; }

        public string LastWebShareErrorMessage { get; private set; }

        public string LastWebShareUrl { get; private set; }

        public string LastWebSharePin { get; private set; }

        public List<ISendTaskV2> SendTasksV2 { get; private set; } = new List<ISendTaskV2>();

        #endregion Public Properties

        #region Public Methods

        public async Task<ISendTaskV2> CreateSendTaskV2(IStorageFile file) {
            IFileRequestDataV2 fileRequestData = await _fileRequestDataManager.CreateFromStorageFileV2Async(file);
            _sendManager.SendCreatedEvent();
            return CreateSendTaskFromFileRequestDataAndStorageFileV2(fileRequestData, file);
        }

        public ISendTaskV2 CreateSendTaskV2(string text) {
            SendTaskV2 sendTask = new SendTaskV2();
            sendTask.File = _universalSendFileManager.CreateUniversalSendFileFromTextV2(text);
            _sendManager.SendCreatedEvent();
            return sendTask;
        }

        public ISendTaskV2 CreateSendTaskFromFileRequestDataAndStorageFileV2(IFileRequestDataV2 fileRequestData, IStorageFile storageFile) {
            SendTaskV2 sendTask = new SendTaskV2();
            IUniversalSendFileV2 universalSendFile = _universalSendFileManager.GetUniversalSendFileFromFileRequestDataV2(fileRequestData); // Token will be added after receiver responds
            sendTask.File = universalSendFile;
            sendTask.StorageFile = storageFile;
            return sendTask;
        }

        public async Task CreateSendTasksV2(List<IStorageFile> files) {
            SendTasksV2.Clear();
            foreach (var file in files) {
                IFileRequestDataV2 fileRequestData = await _fileRequestDataManager.CreateFromStorageFileV2Async(file);
                SendTasksV2.Add(CreateSendTaskFromFileRequestDataAndStorageFileV2(fileRequestData, file));
            }
            _sendManager.SendCreatedEvent();
        }

        public void PublishForWebShare() {
            ClearWebShare();

            string ipAddress = ProgramData.LocalDevice.IP;
            if (string.IsNullOrWhiteSpace(ipAddress)) {
                ipAddress = _networkHelper.GetPrimaryIPv4Address();
                ProgramData.LocalDevice.IP = ipAddress;
            }

            // Read PIN from settings — only if "Require PIN" is enabled
            string pin = null;
            var requirePinSetting = _settings.GetSettingContent(Strings.Constants.WebShare_RequirePin);
            bool requirePin = requirePinSetting is bool b ? b : bool.TryParse(requirePinSetting?.ToString(), out bool parsed) && parsed;
            if (requirePin) {
                pin = _settings.GetSettingContentAsString(Strings.Constants.WebShare_Pin);
                if (string.IsNullOrWhiteSpace(pin)) {
                    // Generate a new PIN if none is set
                    pin = new Random().Next(100000, 999999).ToString();
                    _settings.SetSetting(Strings.Constants.WebShare_Pin, pin);
                }
            }

            bool created = _webSendManager.BeginShare(SendTasksV2, pin);
            LastWebShareUrl = created ? _webSendManager.GetBrowserDownloadUrl(ProgramData.LocalDevice.Port, ipAddress) : string.Empty;
            LastWebSharePin = created ? pin : string.Empty;
            LastWebShareErrorMessage = created
                ? string.Empty
                : "Unable to create a browser download share.";

            if (created && string.IsNullOrWhiteSpace(LastWebShareUrl)) {
                LastWebShareErrorMessage = "Unable to determine a reachable local IP address for the browser download share.";
            }
        }

        public void ClearWebShare(string sessionId = null) {
            _webSendManager.ClearShare(sessionId);
            if (string.IsNullOrWhiteSpace(sessionId) || string.IsNullOrWhiteSpace(LastWebShareUrl)) {
                LastWebShareUrl = string.Empty;
                LastWebSharePin = string.Empty;
            }
            LastWebShareErrorMessage = string.Empty;
        }

        public async Task<bool> SendSendRequestV2Async(IDevice destinationDevice) {
            ClearWebShare();
            LastPrepareUploadStatusCode = 0;
            LastPrepareUploadErrorMessage = null;

            SendRequestDataV2 sendRequestData = new SendRequestDataV2();
            sendRequestData.Files = new Dictionary<string, FileRequestDataV2>();
            sendRequestData.Info = _infoDataManager.GetInfoDataV2FromDevice();
            foreach (var task in SendTasksV2) {
                IFileRequestDataV2 fileRequestData = _fileRequestDataManager.CreateFromUniversalSendFileV2(task.File);
                sendRequestData.Files.Add(task.File.Id, (FileRequestDataV2)fileRequestData);
            }

            var serializedSendRequestData = JsonConvert.SerializeObject(sendRequestData);
            _logger.Debug("SendSendRequestV2Async sending prepare-upload request to http://{0}:{1}/api/localsend/v2/prepare-upload with payload: {2}", destinationDevice.IP, destinationDevice.Port, serializedSendRequestData);

            HttpRequestResult response = await _httpClientHelper.PostAsync(
                $"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v2/prepare-upload",
                new StringContent(serializedSendRequestData, System.Text.Encoding.UTF8, "application/json")
            );
            _logger.Debug("SendSendRequestV2Async received status {0} and response: {1}", response.StatusCode, response.Content);

            LastPrepareUploadStatusCode = response.StatusCode;

            if (response.StatusCode == 204) {
                // LocalSend uses 204 Finished to indicate that no upload phase is needed.
                // This is valid for text/message transfers where the receiver already processed
                // the preview content during prepare-upload.
                if (SendTasksV2.Count > 0 && SendTasksV2.All(IsTextSendTask)) {
                    LastPrepareUploadErrorMessage = null;
                    return true;
                }

                LastPrepareUploadErrorMessage = "The receiver did not accept any files from this transfer.";
                return false;
            }

            if (!response.IsSuccessStatusCode) {
                LastPrepareUploadErrorMessage = GetPrepareUploadErrorMessage(response.StatusCode);
                return false;
            }

            try {
                FileResponseDataV2 fileResponseData = JsonConvert.DeserializeObject<FileResponseDataV2>(response.Content);
                if (fileResponseData == null) {
                    LastPrepareUploadErrorMessage = "The receiver returned an invalid response.";
                    return false;
                }

                foreach (var data in fileResponseData.Files) {
                    ISendTaskV2 sendTask = SendTasksV2.Find(x => x.File.Id == data.Key);
                    sendTask.SessionId = fileResponseData.SessionId;
                    if (sendTask != null) {
                        sendTask.File.TransferToken = data.Value;
                    }
                }
                return true;
            } catch (JsonException) {
                LastPrepareUploadErrorMessage = "The receiver returned an invalid response.";
                return false;
            }
        }

        public async Task SendSendTasksV2Async(IDevice destinationDevice) {
            // /api/localsend/v2/upload?sessionId=some session id&fileId=some file id&token=some token
            ClearWebShare();
            _sendManager.SendStartedEvent();

            if (LastPrepareUploadStatusCode == 204 && SendTasksV2.Count > 0 && SendTasksV2.All(IsTextSendTask)) {
                foreach (ISendTaskV2 task in SendTasksV2) {
                    task.TaskState = ReceiveTaskStates.Done;
                    _sendManager.SendStateChangedEvent();
                }
                return;
            }

            foreach (ISendTaskV2 task in SendTasksV2) {
                if (string.IsNullOrEmpty(task.File.TransferToken)) {
                    task.TaskState = ReceiveTaskStates.Error;
                    _sendManager.SendStateChangedEvent();
                    continue;
                }

                _logger.Debug("SendSendTasksV2Async sending file '{0}'.", task.File.FileName);
                task.TaskState = ReceiveTaskStates.Sending;

                HttpRequestResult uploadResponse;
                if (IsTextSendTask(task)) {
                    uploadResponse = await _httpClientHelper.PostAsync(
                        $"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v2/upload?sessionId={task.SessionId}&fileId={task.File.Id}&token={task.File.TransferToken}",
                        new StringContent(task.File.Preview)
                    );
                } else {
                    using (Stream stream = await _storageHelper.OpenReadStreamAsync(task.StorageFile)) {
                        uploadResponse = await _httpClientHelper.PostAsync(
                            $"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v2/upload?sessionId={task.SessionId}&fileId={task.File.Id}&token={task.File.TransferToken}",
                            new StreamContent(stream)
                        );
                    }
                }

                task.TaskState = uploadResponse.IsSuccessStatusCode ? ReceiveTaskStates.Done : ReceiveTaskStates.Error;
                _sendManager.SendStateChangedEvent();
            }

            ClearWebShare();
        }

        #endregion Public Methods

        #region Private Methods

        private static bool IsTextSendTask(ISendTaskV2 task) {
            return task.StorageFile == null || string.Equals(task.File.FileType, "text/plain", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetPrepareUploadErrorMessage(int statusCode) {
            switch (statusCode) {
                case 400:
                    return "The receiver rejected the transfer request because it was invalid.";
                case 401:
                    return "The receiver requires a PIN or rejected the provided PIN.";
                case 403:
                    return "The receiver declined the transfer request.";
                case 409:
                    return "The receiver is busy with another transfer session.";
                case 429:
                    return "The receiver is temporarily rate limiting transfer requests.";
                case 500:
                    return "The receiver encountered an internal error while preparing the transfer.";
                case 0:
                    return "Failed to contact the receiver.";
                default:
                    return "The transfer request failed.";
            }
        }

        #endregion Private Methods

    }
}