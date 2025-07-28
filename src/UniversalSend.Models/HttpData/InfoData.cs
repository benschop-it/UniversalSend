using Newtonsoft.Json;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.HttpData {

    public class InfoData {

        #region Public Properties

        [JsonProperty("alias")]
        public string Alias { get; set; }
        [JsonProperty("deviceModel")]
        public string DeviceModel { get; set; }
        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }

        #endregion Public Properties
    }

    public class InfoDataManager {

        #region Public Methods

        public static InfoData GetInfoDataFromDevice(Device device) {
            InfoData infoData = new InfoData();
            infoData.Alias = device.Alias;
            infoData.DeviceType = device.DeviceType;
            infoData.DeviceModel = device.DeviceModel;
            return infoData;
        }

        #endregion Public Methods
    }
}