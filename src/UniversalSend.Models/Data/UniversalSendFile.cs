using Newtonsoft.Json;
using System;
using System.Text;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Data {

    public class UniversalSendFile {

        #region Public Properties

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("transferToken")]
        public string TransferToken { get; set; }

        #endregion Public Properties
    }

    public class UniversalSendFileManager {

        #region Public Methods

        public static UniversalSendFile CreateUniversalSendFileFromText(string text) {
            UniversalSendFile universalSendFile = new UniversalSendFile();
            universalSendFile.FileType = "text";
            universalSendFile.Text = text;
            universalSendFile.Size = Encoding.ASCII.GetBytes(text).Length;
            universalSendFile.Id = Guid.NewGuid().ToString();
            universalSendFile.FileName = universalSendFile.Id + ".txt";
            return universalSendFile;
        }

        public static UniversalSendFile GetUniversalSendFileFromFileRequestData(FileRequestData fileRequestData) {
            UniversalSendFile universalSendFile = new UniversalSendFile();
            universalSendFile.Id = fileRequestData.Id;
            universalSendFile.FileName = fileRequestData.FileName;
            universalSendFile.FileType = fileRequestData.FileType;
            universalSendFile.Size = fileRequestData.Size;
            return universalSendFile;
        }

        public static UniversalSendFile GetUniversalSendFileFromFileRequestDataAndToken(FileRequestData fileRequestData, string token) {
            UniversalSendFile universalSendFile = new UniversalSendFile();
            universalSendFile.Id = fileRequestData.Id;
            universalSendFile.FileName = fileRequestData.FileName;
            universalSendFile.FileType = fileRequestData.FileType;
            universalSendFile.Size = fileRequestData.Size;
            universalSendFile.TransferToken = token;
            return universalSendFile;
        }

        #endregion Public Methods
    }
}