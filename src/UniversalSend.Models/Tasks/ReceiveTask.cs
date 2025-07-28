using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;
using Windows.Storage;

namespace UniversalSend.Models.Tasks {

    public class ReceiveTask {

        #region Public Enums

        public enum ReceiveTaskStates {

            [Description("Waiting")]
            Waiting,

            [Description("Transferring")]
            Receiving,

            [Description("Canceled")]
            Canceled,

            [Description("Error")]
            Error,

            [Description("Completed")]
            Done
        }

        #endregion Public Enums

        #region Public Properties

        public UniversalSendFile File { get; set; }

        public byte[] FileContent { get; set; }

        public InfoData Sender { get; set; }

        public ReceiveTaskStates TaskState { get; set; } = ReceiveTaskStates.Waiting;

        #endregion Public Properties
    }

    public class ReceiveTaskManager {

        #region Public Properties

        public static List<ReceiveTask> ReceivingTasks { get; private set; } = new List<ReceiveTask>();

        #endregion Public Properties

        #region Public Methods

        public static void CreateReceivingTaskFromUniversalSendFile(UniversalSendFile universalSendFile, InfoData info) {
            ReceiveTask receiveTask = new ReceiveTask { File = universalSendFile, Sender = info };
            ReceivingTasks.Add(receiveTask);
        }

        public static ReceiveTask WriteFileContentToReceivingTask(string fileId, string token, byte[] fileContent) {
            ReceiveTask task = ReceivingTasks.Find(x => x.File.Id == fileId);
            if (task == null || task.File.TransferToken != token) {
                // task.TaskState = ReceiveTask.ReceiveTaskStates.Error;
                return null;
            }
            task.FileContent = fileContent;
            task.TaskState = ReceiveTask.ReceiveTaskStates.Done;
            return task;
        }

        public static async Task<StorageFile> WriteReceiveTaskToFileAsync(ReceiveTask receiveTask) {
            StorageFile storageFile;
            StorageFolder folder = await StorageHelper.GetReceiveStorageFolderAsync();
            if (folder == null) {
                storageFile = await StorageHelper.CreateFileInDownloadsFolderAsync(receiveTask.File.FileName);
            } else {
                storageFile = await folder.CreateFileAsync(receiveTask.File.FileName);
            }
            await StorageHelper.WriteBytesToFileAsync(storageFile, receiveTask.FileContent);
            return storageFile;
        }

        #endregion Public Methods
    }
}