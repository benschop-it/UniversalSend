using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Interfaces.Internal;

namespace UniversalSend.Models.Managers {
    internal class RegisterRequestDataManager : IRegisterRequestDataManager {

        #region Public Methods

        public IRegisterRequestDataV2 CreateFromDevice(IDevice device) {
            RegisterRequestDataV2 registerRequestData = new RegisterRequestDataV2();
            registerRequestData.Alias = device.Alias;
            registerRequestData.Version = device.ProtocolVersion;
            registerRequestData.DeviceModel = device.DeviceModel;
            registerRequestData.DeviceType = device.DeviceType;
            registerRequestData.Fingerprint = device.Fingerprint;
            registerRequestData.Port = device.Port;
            registerRequestData.Protocol = device.HttpProtocol;
            registerRequestData.Download = true;
            return registerRequestData;
        }

        #endregion Public Methods

    }
}