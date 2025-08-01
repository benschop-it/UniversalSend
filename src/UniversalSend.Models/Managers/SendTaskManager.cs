using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Managers {

    internal class SendTaskManager : ISendTaskManager {

        private IUniversalSendFileManager _universalSendFileManager;
        private IInfoDataManager _infoDataManager;
        private IStorageHelper _storageHelper;
        private ISendTask _sendTask;
        private ISendManager _sendManager;
        private IHttpClientHelper _httpClientHelper;
        private IFileRequestDataManager _fileRequestDataManager;

        public SendTaskManager(
            IUniversalSendFileManager universalSendFileManager,
            IInfoDataManager infoDataManager,
            IStorageHelper storageHelper,
            ISendTask sendTask,
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

        #region Public Properties

        public List<ISendTask> SendTasks { get; private set; } = new List<ISendTask>();

        #endregion Public Properties

        #region Public Methods

        public async Task<ISendTask> CreateSendTask(IStorageFile file) {
            IFileRequestData fileRequestData = await _fileRequestDataManager.CreateFromStorageFileAsync(file);
            _sendManager.SendCreatedEvent();
            return CreateSendTaskFromFileRequestDataAndStorageFile(fileRequestData, file);
        }

        public ISendTask CreateSendTask(string text) {
            SendTask sendTask = new SendTask();
            sendTask.File = _universalSendFileManager.CreateUniversalSendFileFromText(text);
            _sendManager.SendCreatedEvent();
            return sendTask;
        }

        public ISendTask CreateSendTaskFromFileRequestDataAndStorageFile(IFileRequestData fileRequestData, IStorageFile storageFile) {
            SendTask sendTask = new SendTask();
            IUniversalSendFile universalSendFile = _universalSendFileManager.GetUniversalSendFileFromFileRequestData(fileRequestData); // Token will be added after receiver responds
            sendTask.File = universalSendFile;
            sendTask.StorageFile = storageFile;
            return sendTask;
        }

        public async Task CreateSendTasks(List<IStorageFile> files) {
            SendTasks.Clear();
            foreach (var file in files) {
                IFileRequestData fileRequestData = await _fileRequestDataManager.CreateFromStorageFileAsync(file);
                SendTasks.Add(CreateSendTaskFromFileRequestDataAndStorageFile(fileRequestData, file));
            }
            _sendManager.SendCreatedEvent();
        }

        public async Task<bool> SendSendRequestAsync(IDevice destinationDevice) {
            SendRequestData sendRequestData = new SendRequestData();
            sendRequestData.Files = new Dictionary<string, FileRequestData>();
            sendRequestData.Info = _infoDataManager.GetInfoDataFromDevice();
            foreach (var task in SendTasks) {
                IFileRequestData fileRequestData = _fileRequestDataManager.CreateFromUniversalSendFile(task.File);
                sendRequestData.Files.Add(task.File.Id, (FileRequestData)fileRequestData);
            }
            Debug.WriteLine($"Sending send request:\nURL: {destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v1/send-request");

            var serializedSendRequestData = JsonConvert.SerializeObject(sendRequestData);

            Debug.WriteLine($"SendSendRequestAsync: {serializedSendRequestData}");
            string responseStr = await _httpClientHelper.PostJsonAsync($"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v1/send-request", serializedSendRequestData);
            Debug.WriteLine($"Receiver responded: {responseStr}");

            try {
                FileResponseData fileResponseData = JsonConvert.DeserializeObject<FileResponseData>(responseStr);
                if (fileResponseData == null) {
                    SendTasks.Clear();
                    return false;
                }

                foreach (var data in fileResponseData) {
                    ISendTask sendTask = SendTasks.Find(x => x.File.Id == data.Key);
                    if (sendTask != null) {
                        sendTask.File.TransferToken = data.Value;
                    }
                }
                return true;
            } catch (JsonException) {
                return false;
            }
        }

        public async Task SendSendTasksAsync(IDevice destinationDevice) {
            // /api/localsend/v1/send?fileId=some file id&token=some token
            _sendManager.SendStartedEvent();
            foreach (var task in SendTasks) {
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
                        new StringContent(task.File.Text)
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

        #endregion Public Methods
    }
}