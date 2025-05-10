using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
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
            //[Description("等待")]
            Wating,
            //[Description("已取消")]
            Canceled,
            //[Description("错误")]
            Error,
            //[Description("完成")]
            Done
        }

        public ReceiveTaskStates TaskState { get; set; } = ReceiveTaskStates.Wating;
    }

    

    public class ReceiveTaskManager
    {
        public static List<ReceiveTask> ReceivingTasks { get; private set; } = new List<ReceiveTask>();

        public static void CreateReceivingTaskFromUniversalSendFile(UniversalSendFile universalSendFile)
        {
            ReceiveTask receiveTask = new ReceiveTask { file = universalSendFile};
            ReceivingTasks.Add(receiveTask);
            
        }

        public static ReceiveTask WriteFileContentToReceivingTask(string fileId,string token, byte[] fileContent)
        {
            ReceiveTask task = ReceivingTasks.Find(x=>x.file.Id == fileId);
            if (task == null || task.file.TransferToken != token)
            {
                //task.TaskState = ReceiveTask.ReceiveTaskStates.Error;
                return null;
            }
            task.fileContent = fileContent;
            task.TaskState = ReceiveTask.ReceiveTaskStates.Done;
            return task;
        }

        public static async Task<StorageFile> WriteReceiveTaskToFileAsync(ReceiveTask receiveTask)
        {

            //StorageFile storageFile = await StorageHelper.CreateFileInAppLocalFolderAsync(receiveTask.file.FileName);
            StorageFile storageFile = await StorageHelper.CreateFileInDownloadsFolderAsync(receiveTask.file.FileName);
            await StorageHelper.WriteBytesToFileAsync(storageFile,receiveTask.fileContent);
            return storageFile;
        }
    }
}
