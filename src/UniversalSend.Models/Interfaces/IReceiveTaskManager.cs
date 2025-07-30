using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {
    public interface IReceiveTaskManager {
        List<IReceiveTask> ReceivingTasks { get; }

        void CreateReceivingTaskFromUniversalSendFile(IUniversalSendFile universalSendFile, IInfoData info);

        IReceiveTask WriteFileContentToReceivingTask(string fileId, string token, byte[] fileContent);

        Task<IStorageFile> WriteReceiveTaskToFileAsync(IReceiveTask receiveTask);
    }
}