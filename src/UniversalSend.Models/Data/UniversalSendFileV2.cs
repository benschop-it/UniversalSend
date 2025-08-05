using Newtonsoft.Json;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Interfaces;

namespace UniversalSend.Models.Data {

    internal class UniversalSendFileV2 : IUniversalSendFileV2 {

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

        [JsonProperty("transferToken")]
        public string TransferToken { get; set; }

        #endregion Public Properties

    }
}