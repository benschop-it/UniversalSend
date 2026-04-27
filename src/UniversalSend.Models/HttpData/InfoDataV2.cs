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

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

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