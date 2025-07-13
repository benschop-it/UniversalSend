using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Streams;

namespace UniversalSend.Models.Tasks
{
    public class SendTask
    {
        public UniversalSendFile File { get; set; }

        public StorageFile StorageFile { get; set; }

        public enum ReceiveTaskStates
        {
            [Description("Waiting")]
            Wating,
            [Description("Transferring")]
            Sending,
            [Description("Canceled")]
            Canceled,
            [Description("Error")]
            Error,
            [Description("Completed")]
            Done
        }

        public ReceiveTaskStates TaskState { get; set; } = ReceiveTaskStates.Wating;
    }

    public class SendTaskManager
    {
        public static List<SendTask> SendTasks { get; private set; } = new List<SendTask>();

        public static SendTask CreateSendTaskFromFileRequestDataAndStorageFile(FileRequestData fileRequestData, StorageFile storageFile)
        {
            SendTask sendTask = new SendTask();
            UniversalSendFile universalSendFile = UniversalSendFileManager.GetUniversalSendFileFromFileRequestData(fileRequestData); // Token will be added after receiver responds
            sendTask.File = universalSendFile;
            sendTask.StorageFile = storageFile;
            return sendTask;
        }

        public static async Task CreateSendTasks(List<StorageFile> files)
        {
            SendTasks.Clear();
            foreach (var file in files)
            {
                FileRequestData fileRequestData = await FileRequestDataManager.CreateFromStorageFileAsync(file);
                SendTasks.Add(CreateSendTaskFromFileRequestDataAndStorageFile(fileRequestData, file));
            }
            SendManager.SendCreatedEvent();
        }

        public static async Task<SendTask> CreateSendTask(StorageFile file)
        {
            FileRequestData fileRequestData = await FileRequestDataManager.CreateFromStorageFileAsync(file);
            SendManager.SendCreatedEvent();
            return CreateSendTaskFromFileRequestDataAndStorageFile(fileRequestData, file);
        }

        public static SendTask CreateSendTask(string text)
        {
            SendTask sendTask = new SendTask();
            sendTask.File = UniversalSendFileManager.CreateUniversalSendFileFromText(text);
            SendManager.SendCreatedEvent();
            return sendTask;
        }

        public static async Task<bool> SendSendRequestAsync(Device destinationDevice) // Send request
        {
            SendRequestData sendRequestData = new SendRequestData();
            sendRequestData.files = new Dictionary<string, FileRequestData>();
            sendRequestData.info = InfoDataManager.GetInfoDataFromDevice(ProgramData.LocalDevice);
            foreach (var task in SendTasks)
            {
                sendRequestData.files.Add(task.File.Id, FileRequestDataManager.CreateFromUniversalSendFile(task.File));
            }
            Debug.WriteLine($"Sending send request:\nURL: {destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v1/send-request");
            Debug.WriteLine(JsonConvert.SerializeObject(sendRequestData));
            string responseStr = await HttpClientHelper.PostJsonAsync(
                $"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v1/send-request",
                JsonConvert.SerializeObject(sendRequestData)
            );
            Debug.WriteLine($"Receiver responded: {responseStr}");
            try
            {
                FileResponseData fileResponseData = JsonConvert.DeserializeObject<FileResponseData>(responseStr);
                if (fileResponseData == null)
                {
                    SendTasks.Clear();
                    return false;
                }

                foreach (var data in fileResponseData)
                {
                    SendTask sendTask = SendTasks.Find(x => x.File.Id == data.Key);
                    if (sendTask != null)
                    {
                        sendTask.File.TransferToken = data.Value;
                    }
                }
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        public static async Task SendSendTasksAsync(Device destinationDevice)
        {
            // /api/localsend/v1/send?fileId=some file id&token=some token
            SendManager.SendStartedEvent();
            foreach (var task in SendTasks)
            {
                Debug.WriteLine($"Preparing to send file: {task.File.FileName}");

                if (string.IsNullOrEmpty(task.File.TransferToken))
                {
                    task.TaskState = SendTask.ReceiveTaskStates.Error;
                    SendManager.SendStateChangedEvent();
                    continue;
                }

                Debug.WriteLine($"Sending file: {task.File.FileName}");
                task.TaskState = SendTask.ReceiveTaskStates.Sending;

                if (task.File.FileType == "text")
                {
                    await HttpClientHelper.PostStringAsync(
                        $"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v1/send?fileId={task.File.Id}&token={task.File.TransferToken}",
                        new StringContent(task.File.Text)
                    );
                }
                else
                {
                    byte[] bytes = await StorageHelper.ReadBytesFromFileAsync(task.StorageFile);
                    await HttpClientHelper.PostBinaryAsync(
                        $"http://{destinationDevice.IP}:{destinationDevice.Port}/api/localsend/v1/send?fileId={task.File.Id}&token={task.File.TransferToken}",
                        bytes
                    );
                }

                task.TaskState = SendTask.ReceiveTaskStates.Done;
                SendManager.SendStateChangedEvent();
            }
        }
    }
}
