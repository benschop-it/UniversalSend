using System;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using Windows.Storage;

namespace UniversalSend.Models.Managers {

    internal class FileRequestDataManager : IFileRequestDataManager {

        #region Public Methods

        public async Task<IFileRequestDataV2> CreateFromStorageFileV2Async(IStorageFile storageFile) {
            FileRequestDataV2 fileRequestData = new FileRequestDataV2();
            fileRequestData.FileType = storageFile.FileType;
            fileRequestData.Size = (long)(await storageFile.GetBasicPropertiesAsync()).Size;
            fileRequestData.FileName = storageFile.Name;
            fileRequestData.Id = Guid.NewGuid().ToString();
            return fileRequestData;
        }

        public IFileRequestDataV2 CreateFromUniversalSendFileV2(IUniversalSendFileV2 universalSendFile) {
            FileRequestDataV2 fileRequestData = new FileRequestDataV2();
            fileRequestData.Id = universalSendFile.Id;
            fileRequestData.FileName = universalSendFile.FileName;
            fileRequestData.FileType = universalSendFile.FileType;
            fileRequestData.Size = universalSendFile.Size;
            if (!string.IsNullOrEmpty(universalSendFile.Sha256)) {
                fileRequestData.Sha256 = universalSendFile.Sha256;
            }
            if (!string.IsNullOrEmpty(universalSendFile.Preview)) {
                fileRequestData.Preview = universalSendFile.Preview;
            }
            return fileRequestData;
        }


        #endregion Public Methods

    }
}