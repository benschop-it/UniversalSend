using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace UniversalSend.Models.Tasks
{
    public class ReceiveTask
    {
        public UniversalSendFile file { get; set; }
        public byte[] fileContent { get; set; }

        public enum ReceiveTaskStates 
        {
            [Description("Waiting")]
            Wating,
            [Description("Transferring")]
            Receiving,
            [Description("Canceled")]
            Canceled,
            [Description("Error")]
            Error,
            [Description("Completed")]
            Done
        }

        public InfoData sender { get; set; }

        public ReceiveTaskStates TaskState { get; set; } = ReceiveTaskStates.Wating;
    }

    public class ReceiveTaskManager
    {
        public static List<ReceiveTask> ReceivingTasks { get; private set; } = new List<ReceiveTask>();

        public static void CreateReceivingTaskFromUniversalSendFile(UniversalSendFile universalSendFile, InfoData info)
        {
            ReceiveTask receiveTask = new ReceiveTask { file = universalSendFile, sender = info };
            ReceivingTasks.Add(receiveTask);
        }

        public static ReceiveTask WriteFileContentToReceivingTask(string fileId, string token, byte[] fileContent)
        {
            ReceiveTask task = ReceivingTasks.Find(x => x.file.Id == fileId);
            if (task == null || task.file.TransferToken != token)
            {
                // task.TaskState = ReceiveTask.ReceiveTaskStates.Error;
                return null;
            }
            task.fileContent = fileContent;
            task.TaskState = ReceiveTask.ReceiveTaskStates.Done;
            return task;
        }

        public static async Task<StorageFile> WriteReceiveTaskToFileAsync(ReceiveTask receiveTask)
        {
            StorageFile storageFile;
            // StorageFile storageFile = await StorageHelper.CreateFileInAppLocalFolderAsync(receiveTask.file.FileName);
            StorageFolder folder = await StorageHelper.GetReceiveStoageFolderAsync();
            if (folder == null)
            {
                storageFile = await StorageHelper.CreateFileInDownloadsFolderAsync(receiveTask.file.FileName);
            }
            else
            {
                storageFile = await folder.CreateFileAsync(receiveTask.file.FileName);
            }
            await StorageHelper.WriteBytesToFileAsync(storageFile, receiveTask.fileContent);
            return storageFile;
        }
    }
}
