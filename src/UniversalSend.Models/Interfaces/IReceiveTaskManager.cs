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

        Task CreateReceivingTaskFromUniversalSendFileV2Async(IUniversalSendFileV2 universalSendFile, IInfoDataV2 info);

        void CreateReceivingTaskFromUniversalSendFileV2(IUniversalSendFileV2 universalSendFile, IInfoDataV2 info);

        Task<IReceiveTask> WriteFileContentToReceivingTaskV2(string sessionId, string fileId, string token, byte[] fileContent);

        Task<IStorageFile> WriteReceiveTaskToFileV2Async(IReceiveTask receiveTask);

        #endregion Public Methods
    }
}