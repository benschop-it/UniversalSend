﻿using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Interfaces.Internal;

namespace UniversalSend.Models.Managers {
    internal class RegisterRequestDataManager : IRegisterRequestDataManager {

        #region Public Methods

        public IRegisterRequestData CreateFromDevice(IDevice device) {
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