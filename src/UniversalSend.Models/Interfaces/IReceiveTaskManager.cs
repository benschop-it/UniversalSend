using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {
    public interface IReceiveTaskManager {

        #region Public Properties

        List<IReceiveTask> ReceivingTasks { get; }

        #endregion Public Properties

        #region Public Methods

        void CreateReceivingTaskFromUniversalSendFile(IUniversalSendFile universalSendFile, IInfoData info);

        IReceiveTask WriteFileContentToReceivingTask(string fileId, string token, byte[] fileContent);

        Task<IStorageFile> WriteReceiveTaskToFileAsync(IReceiveTask receiveTask);

        #endregion Public Methods
    }
}