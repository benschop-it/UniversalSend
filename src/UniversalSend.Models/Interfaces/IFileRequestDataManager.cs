using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {
    public interface IFileRequestDataManager {

        #region Public Methods

        Task<IFileRequestDataV2> CreateFromStorageFileV2Async(IStorageFile storageFile);

        IFileRequestDataV2 CreateFromUniversalSendFileV2(IUniversalSendFileV2 universalSendFile);

        #endregion Public Methods
    }
}