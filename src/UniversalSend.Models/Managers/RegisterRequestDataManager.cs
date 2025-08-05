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
            registerRequestData.DeviceModel = device.DeviceModel;
            registerRequestData.DeviceType = device.DeviceType;
            registerRequestData.Fingerprint = device.Fingerprint;
            registerRequestData.Protocol = device.ProtocolVersion;
            return registerRequestData;
        }

        #endregion Public Methods

    }
}