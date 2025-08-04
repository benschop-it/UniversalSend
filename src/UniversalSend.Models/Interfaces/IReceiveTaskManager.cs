using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {
    public interface IReceiveTaskManager {

        #region Public Properties

        ObservableCollection<IReceiveTask> ReceivingTasks { get; }

        #endregion Public Properties

        #region Public Methods

        Task CreateReceivingTaskFromUniversalSendFileAsync(IUniversalSendFile universalSendFile, IInfoData info);

        void CreateReceivingTaskFromUniversalSendFile(IUniversalSendFile universalSendFile, IInfoData info);

        Task<IReceiveTask> WriteFileContentToReceivingTask(string fileId, string token, byte[] fileContent);

        Task<IStorageFile> WriteReceiveTaskToFileAsync(IReceiveTask receiveTask);

        #endregion Public Methods
    }
}