using Newtonsoft.Json;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    internal class RegisterRequestData : IRegisterRequestData {

        #region Public Properties

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("deviceModel")]
        public string DeviceModel { get; set; }

        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        #endregion Public Properties
    }

    internal class RegisterRequestDataManager {

        #region Public Methods

        public static RegisterRequestData CreateFromDevice(Device device) {
            RegisterRequestData registerRequestData = new RegisterRequestData();
            registerRequestData.Alias = device.Alias;
            registerRequestData.DeviceModel = device.DeviceModel;
            registerRequestData.DeviceType = device.DeviceType;
            registerRequestData.Fingerprint = device.Fingerprint;
            return registerRequestData;
        }

        #endregion Public Methods
    }
}