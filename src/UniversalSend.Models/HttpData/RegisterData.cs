using Newtonsoft.Json;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.HttpData {

    public sealed class RegisterData {

        #region Public Properties

        [JsonProperty("alias")]
        public string Alias { get; set; }
        [JsonProperty("announce")]
        public bool Announce { get; set; }
        [JsonProperty("deviceModel")]
        public string DeviceModel { get; set; }
        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }
        [JsonProperty("download")]
        public bool Download { get; set; }
        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }
        [JsonProperty("port")]
        public int Port { get; set; }
        [JsonProperty("protocol")]
        public string Protocol { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }

        #endregion Public Properties
    }

    public class RegisterDataManager {

        #region Public Methods

        /// <summary>
        /// Gets RegisterData from a Device instance, commonly used to retrieve local device registration information
        /// </summary>
        public static RegisterData GetRegisterDataFromDevice(Device device) {
            RegisterData registerData = new RegisterData();
            registerData.Alias = device.Alias;
            registerData.Version = device.ProtocolVersion;
            registerData.DeviceModel = device.DeviceModel;
            registerData.DeviceType = device.DeviceType;
            registerData.Fingerprint = device.Fingerprint;
            registerData.Port = device.Port;
            registerData.Protocol = device.HttpProtocol;
            registerData.Download = true;
            registerData.Announce = false;
            return registerData;
        }

        #endregion Public Methods
    }
}