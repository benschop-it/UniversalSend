using Newtonsoft.Json;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    public class InfoDataV2 : IInfoDataV2 {

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
        public string FingerPrint { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("download")]
        public bool Download { get; set; }

        #endregion Public Properties

    }
}