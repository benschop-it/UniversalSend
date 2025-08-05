using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Interfaces.Internal;
using UniversalSend.Models.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Managers {

    internal class SendTaskManager : ISendTaskManager {

        #region Private Fields

        private IFileRequestDataManager _fileRequestDataManager;
        private IHttpClientHelper _httpClientHelper;
        private IInfoDataManager _infoDataManager;
        private ISendManager _sendManager;
        private ISendTaskV1 _sendTask;
        private IStorageHelper _storageHelper;
        private IUniversalSendFileManager _universalSendFileManager;

        #endregion Private Fields

        #region Public Constructors

        public SendTaskManager(
            IUniversalSendFileManager universalSendFileManager,
            IInfoDataManager infoDataManager,
            IStorageHelper storageHelper,
            ISendTaskV1 sendTask,
            ISendManager sendManager,
            IHttpClientHelper httpClientHelper,
            IFileRequestDataManager fileRequestDataManager
        ) {
            _universalSendFileManager = universalSendFileManager ?? throw new System.ArgumentNullException(nameof(universalSendFileManager));
            _infoDataManager = infoDataManager ?? throw new System.ArgumentNullException(nameof(infoDataManager));
            _storageHelper = storageHelper ?? throw new System.ArgumentNullException(nameof(storageHelper));
            _sendTask = sendTask ?? throw new System.ArgumentNullException(nameof(sendTask));
            _sendManager = sendManager ?? throw new System.ArgumentNullException(nameof(sendManager));
            _httpClientHelper = httpClientHelper ?? throw new ArgumentNullException(nameof(httpClientHelper));
            _fileRequestDataManager = fileRequestDataManager ?? throw new ArgumentNullException(nameof(fileRequestDataManager));
        }

        #endregion Public Constructors

        #region Public Properties

        public List<ISendTaskV1> SendTasksV1 { get; private set; } = new List<ISendTaskV1>();

        public List<ISendTaskV2> SendTasksV2 { get; private set; } = new List<ISendTaskV2>();

        #endregion Public Properties

        #region Public Methods

        public async Task<ISendTaskV1> CreateSendTaskV1(IStorageFile file) {
            IFileRequestDataV1 fileRequestData = await _fileRequestDataManager.CreateFromStorageFileV1Async(file);
            _sendManager.SendCreatedEvent();
            return CreateSendTaskFromFileRequestDataAndStorageFileV1(fileRequestData, file);
        }

        public async Task<ISendTaskV2> CreateSendTaskV2(IStorageFile file) {
            IFileRequestDataV2 fileRequestData = await _fileRequestDataManager.CreateFromStorageFileV2Async(file);
            _sendManager.SendCreatedEvent();
            return CreateSendTaskFromFileRequestDataAndStorageFileV2(fileRequestData, file);
        }

        public ISendTaskV1 CreateSendTaskV1(string text) {
            SendTaskV1 sendTask = new SendTaskV1();
            sendTask.File = _universalSendFileManager.CreateUniversalSendFileFromTextV1(text);
            _sendManager.SendCreatedEvent();
            return sendTask;
        }

        public ISendTaskV2 CreateSendTaskV2(string text) {
            SendTaskV2 sendTask = new SendTaskV2();
            sendTask.File = _universalSendFileManager.CreateUniversalSendFileFromTextV2(text);
            _sendManager.SendCreatedEvent();
            return sendTask;
        }

        public ISendTaskV1 CreateSendTaskFromFileRequestDataAndStorageFileV1(IFileRequestDataV1 fileRequestData, IStorageFile storageFile) {
            SendTaskV1 sendTask = new SendTaskV1();
            IUniversalSendFileV1 universalSendFile = _universalSendFileManager.GetUniversalSendFileFromFileRequestDataV1(fileRequestData); // Token will be added after receiver responds
            sendTask.File = universalSendFile;
            sendTask.StorageFile = storageFile;
            return sendTask;
        }

        public ISendTaskV2 CreateSendTaskFromFileRequestDataAndStorageFileV2(IFileRequestDataV2 fileRequestData, IStorageFile storageFile) {
            SendTaskV2 sendTask = new SendTaskV2();
            IUniversalSendFileV2 universalSendFile = _universalSendFileManager.GetUniversalSendFileFromFileRequestDataV2(fileRequestData); // Token will be added after receiver responds
            sendTask.File = universalSendFile;
            sendTask.StorageFile = storageFile;
            return sendTask;
        }

        public async Task CreateSendTasksV1(List<IStorageFile> files) {
            SendTasksV1.Clear();
            foreach (var file in files) {
                IFileRequestDataV1 fileRequestData = await _fileRequestDataManager.CreateFromStorageFileV1Async(file);
                SendTasksV1.Add(CreateSendTaskFromFileRequestDataAndStorageFileV1(fileRequestData, file));
            }
            _sendManager.SendCreatedEvent();
        }

        public async Task CreateSendTasksV2(List<IStorageFile> files) {
            SendTasksV2.Clear();
            foreach (var file in files) {
                IFileRequestDataV2 fileRequestData = await _fileRequestDataManager.CreateFromStorageFileV2Async(file);
                SendTasksV2.Add(CreateSendTaskFromFileRequestDataAndStorageFileV2(fileRequestData, file));
            }
            _sendManager.SendCreatedEvent();
        }


        public async Task<bool> SendSendRequestV1Async(IDevice destinationDevice) {
            SendRequestDataV1 sendRequestData = new SendRequestDataV1();
            sendRequestData.Files = new Dictionary<string, FileRequestDataV1>();
            sendRequestData.Info = _infoDataManager.GetInfoDataV1FromDevice();
            foreach (var task in SendTasksV1) {
                IFileRequestDataV1 fileRequestData = _fileRequestDataManager.CreateFromUniversalSendFileV1(task.File);
                sendRequestData.Files.Add(task.File.Id, (FileRequestDataV1)fileRequestData);
            }
            Debug.WriteLine($"Sending send request:\nURL: {destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v1/send-request");

            var serializedSendRequestData = JsonConvert.SerializeObject(sendRequestData);

            Debug.WriteLine($"SendSendRequestAsync: {serializedSendRequestData}");
            string responseStr = await _httpClientHelper.PostJsonAsync($"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v1/send-request", serializedSendRequestData);
            Debug.WriteLine($"Receiver responded: {responseStr}");

            try {
                FileResponseData fileResponseData = JsonConvert.DeserializeObject<FileResponseData>(responseStr);
                if (fileResponseData == null) {
                    SendTasksV1.Clear();
                    return false;
                }

                foreach (var data in fileResponseData) {
                    ISendTaskV1 sendTask = SendTasksV1.Find(x => x.File.Id == data.Key);
                    if (sendTask != null) {
                        sendTask.File.TransferToken = data.Value;
                    }
                }
                return true;
            } catch (JsonException) {
                return false;
            }
        }

        public async Task<bool> SendSendRequestV2Async(IDevice destinationDevice) {
            SendRequestDataV2 sendRequestData = new SendRequestDataV2();
            sendRequestData.Files = new Dictionary<string, FileRequestDataV2>();
            sendRequestData.Info = _infoDataManager.GetInfoDataV2FromDevice();
            foreach (var task in SendTasksV2) {
                IFileRequestDataV2 fileRequestData = _fileRequestDataManager.CreateFromUniversalSendFileV2(task.File);
                sendRequestData.Files.Add(task.File.Id, (FileRequestDataV2)fileRequestData);
            }
            Debug.WriteLine($"Sending send request:\nURL: {destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v2/send-request");

            var serializedSendRequestData = JsonConvert.SerializeObject(sendRequestData);

            Debug.WriteLine($"SendSendRequestAsync: {serializedSendRequestData}");
            string responseStr = await _httpClientHelper.PostJsonAsync($"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v2/send-request", serializedSendRequestData);
            Debug.WriteLine($"Receiver responded: {responseStr}");

            try {
                FileResponseData fileResponseData = JsonConvert.DeserializeObject<FileResponseData>(responseStr);
                if (fileResponseData == null) {
                    SendTasksV2.Clear();
                    return false;
                }

                foreach (var data in fileResponseData) {
                    ISendTaskV2 sendTask = SendTasksV2.Find(x => x.File.Id == data.Key);
                    if (sendTask != null) {
                        sendTask.File.TransferToken = data.Value;
                    }
                }
                return true;
            } catch (JsonException) {
                return false;
            }
        }

        public async Task SendSendTasksV1Async(IDevice destinationDevice) {
            // /api/localsend/v1/send?fileId=some file id&token=some token
            _sendManager.SendStartedEvent();
            foreach (var task in SendTasksV1) {
                Debug.WriteLine($"Preparing to send file: {task.File.FileName}");

                if (string.IsNullOrEmpty(task.File.TransferToken)) {
                    task.TaskState = ReceiveTaskStates.Error;
                    _sendManager.SendStateChangedEvent();
                    continue;
                }

                Debug.WriteLine($"Sending file: {task.File.FileName}");
                task.TaskState = ReceiveTaskStates.Sending;

                if (task.File.FileType == "text") {
                    await _httpClientHelper.PostStringAsync(
                        $"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v1/send?fileId={task.File.Id}&token={task.File.TransferToken}",
                        new StringContent(task.File.Preview)
                    );
                } else {
                    byte[] bytes = await _storageHelper.ReadBytesFromFileAsync(task.StorageFile);
                    await _httpClientHelper.PostBinaryAsync(
                        $"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v1/send?fileId={task.File.Id}&token={task.File.TransferToken}",
                        bytes
                    );
                }

                task.TaskState = ReceiveTaskStates.Done;
                _sendManager.SendStateChangedEvent();
            }
        }

        public async Task SendSendTasksV2Async(IDevice destinationDevice) {
            // /api/localsend/v2/upload?sessionId=some session id&fileId=some file id&token=some token
            _sendManager.SendStartedEvent();
            foreach (ISendTaskV2 task in SendTasksV1) {
                Debug.WriteLine($"Preparing to send file: {task.File.FileName}");

                if (string.IsNullOrEmpty(task.File.TransferToken)) {
                    task.TaskState = ReceiveTaskStates.Error;
                    _sendManager.SendStateChangedEvent();
                    continue;
                }

                Debug.WriteLine($"Sending file: {task.File.FileName}");
                task.TaskState = ReceiveTaskStates.Sending;

                if (task.File.FileType == "text") {
                    await _httpClientHelper.PostStringAsync(
                        $"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v2/upload?sessionId={task.SessionId}&fileId={task.File.Id}&token={task.File.TransferToken}",
                        new StringContent(task.File.Preview)
                    );
                } else {
                    byte[] bytes = await _storageHelper.ReadBytesFromFileAsync(task.StorageFile);
                    await _httpClientHelper.PostBinaryAsync(
                        $"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v2/upload?sessionId={task.SessionId}&fileId={task.File.Id}&token={task.File.TransferToken}",
                        bytes
                    );
                }

                task.TaskState = ReceiveTaskStates.Done;
                _sendManager.SendStateChangedEvent();
            }
        }

        #endregion Public Methods

    }
}