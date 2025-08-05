using Newtonsoft.Json;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    public class InfoDataV1 : IInfoDataV1 {

        #region Public Properties

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("deviceModel")]
        public string DeviceModel { get; set; }

        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }

        #endregion Public Properties

    }
}