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

        [JsonIgnore]
        public string Fingerprint { get; set; }

        [JsonProperty("token")]
        public string Token {
            get => Fingerprint;
            set {
                if (string.IsNullOrWhiteSpace(Fingerprint)) {
                    Fingerprint = value;
                }
            }
        }

        [JsonIgnore]
        public bool Download { get; set; }

        [JsonProperty("hasWebInterface")]
        public bool HasWebInterface {
            get => Download;
            set {
                Download = value;
            }
        }

        #endregion Public Properties

    }
}