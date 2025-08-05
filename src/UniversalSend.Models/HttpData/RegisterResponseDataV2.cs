using Newtonsoft.Json;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    internal class RegisterResponseDataV2 : IRegisterResponseDataV2 {

        #region Public Properties

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("deviceModel")]
        public string DeviceModel { get; set; }

        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("download")]
        public bool Download { get; set; }

        #endregion Public Properties

    }
}