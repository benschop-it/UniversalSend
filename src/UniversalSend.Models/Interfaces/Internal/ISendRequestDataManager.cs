using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces.Internal {
    internal interface ISendRequestDataManager {

        #region Public Methods

        Task<ISendRequestDataV1> CreateSendRequestDataV1Async(List<IStorageFile> storageFiles);
        Task<ISendRequestDataV2> CreateSendRequestDataV2Async(List<IStorageFile> storageFiles);

        #endregion Public Methods
    }
}