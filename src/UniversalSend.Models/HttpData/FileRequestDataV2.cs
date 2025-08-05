using Newtonsoft.Json;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    public class FileRequestDataV2 : IFileRequestDataV2 {

        #region Public Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        [JsonProperty("preview")]
        public string Preview { get; set; }

        [JsonProperty("metadata")]
        public MetaData Metadata { get; set; }

        #endregion Public Properties

    }
}