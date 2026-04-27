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
        public string Fingerprint { get; set; }

        /// <summary>Accepts v3 "token" field as alias for fingerprint during deserialization.</summary>
        [JsonProperty("token")]
        private string Token {
            set {
                if (string.IsNullOrWhiteSpace(Fingerprint)) {
                    Fingerprint = value;
                }
            }
        }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("download")]
        public bool Download { get; set; }

        /// <summary>Accepts v3 "hasWebInterface" field as alias for download during deserialization.</summary>
        [JsonProperty("hasWebInterface")]
        private bool HasWebInterface {
            set {
                Download = value;
            }
        }

        #endregion Public Properties

    }
}