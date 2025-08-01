using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {
    public interface IFileRequestDataManager {

        #region Public Methods

        Task<IFileRequestData> CreateFromStorageFileAsync(IStorageFile storageFile);
        IFileRequestData CreateFromUniversalSendFile(IUniversalSendFile universalSendFile);

        #endregion Public Methods
    }
}