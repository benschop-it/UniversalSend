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

        [JsonProperty("token")]
        private string Token {
            set {
                if (string.IsNullOrWhiteSpace(Fingerprint)) {
                    Fingerprint = value;
                }
            }
        }

        [JsonProperty("download")]
        public bool Download { get; set; }

        [JsonProperty("hasWebInterface")]
        private bool HasWebInterface {
            set {
                Download = value;
            }
        }

        #endregion Public Properties

    }
}