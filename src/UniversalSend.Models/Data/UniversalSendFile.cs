using Newtonsoft.Json;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Interfaces;

namespace UniversalSend.Models.Data {

    internal class UniversalSendFile : IUniversalSendFile {

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
}