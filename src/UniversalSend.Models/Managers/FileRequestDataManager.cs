using System;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using Windows.Storage;

namespace UniversalSend.Models.Managers {
    internal class FileRequestDataManager : IFileRequestDataManager {

        #region Public Methods

        public async Task<IFileRequestData> CreateFromStorageFileAsync(IStorageFile storageFile) {
            FileRequestData fileRequestData = new FileRequestData();
            fileRequestData.FileType = storageFile.FileType;
            fileRequestData.Size = (long)(await storageFile.GetBasicPropertiesAsync()).Size;
            fileRequestData.FileName = storageFile.Name;
            fileRequestData.Id = Guid.NewGuid().ToString();
            return fileRequestData;
        }

        public IFileRequestData CreateFromUniversalSendFile(IUniversalSendFile universalSendFile) {
            FileRequestData fileRequestData = new FileRequestData();
            fileRequestData.FileType = universalSendFile.FileType;
            fileRequestData.FileName = universalSendFile.FileName;
            fileRequestData.Size = universalSendFile.Size;
            fileRequestData.Id = universalSendFile.Id;
            if (!string.IsNullOrEmpty(universalSendFile.Text)) {
                fileRequestData.Preview = universalSendFile.Text;
            }
            return fileRequestData;
        }

        #endregion Public Methods
    }
}