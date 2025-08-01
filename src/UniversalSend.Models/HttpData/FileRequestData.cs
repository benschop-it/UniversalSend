using Newtonsoft.Json;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    public class FileRequestData : IFileRequestData {

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
}