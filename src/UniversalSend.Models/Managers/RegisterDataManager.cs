using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    internal class RegisterDataManager : IRegisterDataManager {

        #region Public Methods

        /// <summary>
        /// Gets RegisterData from a Device instance, commonly used to retrieve local device registration information
        /// </summary>
        public IRegisterDataV2 GetRegisterDataV2FromDevice() {
            var device = ProgramData.LocalDevice;

            IRegisterDataV2 registerData = new RegisterDataV2();
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