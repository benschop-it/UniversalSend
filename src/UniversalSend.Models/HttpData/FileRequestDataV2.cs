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

        /// <summary>Accepts the "hash" field used by LocalSend Dart as alias for sha256.</summary>
        [JsonProperty("hash")]
        private string Hash {
            set {
                if (string.IsNullOrWhiteSpace(Sha256)) {
                    Sha256 = value;
                }
            }
        }

        [JsonProperty("preview")]
        public string Preview { get; set; }

        [JsonProperty("metadata")]
        public FileMetadataV2 Metadata { get; set; }

        #endregion Public Properties

    }
}