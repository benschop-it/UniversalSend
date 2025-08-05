using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {
    public interface IFileRequestDataManager {

        #region Public Methods

        Task<IFileRequestDataV1> CreateFromStorageFileV1Async(IStorageFile storageFile);
        Task<IFileRequestDataV2> CreateFromStorageFileV2Async(IStorageFile storageFile);

        IFileRequestDataV1 CreateFromUniversalSendFileV1(IUniversalSendFileV1 universalSendFile);

        IFileRequestDataV2 CreateFromUniversalSendFileV2(IUniversalSendFileV2 universalSendFile);

        #endregion Public Methods
    }
}