using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using Windows.Storage;

namespace UniversalSend.Models.HttpData {

    public class FileRequestData {

        #region Public Properties

        [JsonProperty("fileName")]
        public string FileName { get; set; }
        [JsonProperty("fileType")]
        public string FileType { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("preview")]
        public string Preview { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }

        #endregion Public Properties
    }

    public class FileRequestDataManager {

        #region Public Methods

        public static async Task<FileRequestData> CreateFromStorageFileAsync(StorageFile storageFile) {
            FileRequestData fileRequestData = new FileRequestData();
            fileRequestData.FileType = storageFile.FileType;
            fileRequestData.Size = (long)(await storageFile.GetBasicPropertiesAsync()).Size;
            fileRequestData.FileName = storageFile.Name;
            fileRequestData.Id = Guid.NewGuid().ToString();
            return fileRequestData;
        }

        public static FileRequestData CreateFromUniversalSendFile(UniversalSendFile universalSendFile) {
            FileRequestData fileRequestData = new FileRequestData();
            fileRequestData.FileType = universalSendFile.FileType;
            fileRequestData.FileName = universalSendFile.FileName;
            fileRequestData.Size = universalSendFile.Size;
            fileRequestData.Id = universalSendFile.Id;
            if (!String.IsNullOrEmpty(universalSendFile.Text)) {
                fileRequestData.Preview = universalSendFile.Text;
            }
            return fileRequestData;
        }

        #endregion Public Methods
    }
}