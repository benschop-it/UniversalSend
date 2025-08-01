using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {
    public class RegisterDataManager : IRegisterDataManager {

        #region Public Methods

        /// <summary>
        /// Gets RegisterData from a Device instance, commonly used to retrieve local device registration information
        /// </summary>
        public IRegisterData GetRegisterDataFromDevice() {
            var device = ProgramData.LocalDevice;

            IRegisterData registerData = new RegisterData();
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