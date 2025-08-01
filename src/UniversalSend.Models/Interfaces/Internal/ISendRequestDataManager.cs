using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces.Internal {
    internal interface ISendRequestDataManager {

        #region Public Methods

        Task<ISendRequestData> CreateSendRequestDataAsync(List<IStorageFile> storageFiles);

        #endregion Public Methods
    }
}