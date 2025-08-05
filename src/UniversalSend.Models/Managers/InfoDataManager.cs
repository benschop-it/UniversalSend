using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    internal class InfoDataManager : IInfoDataManager {

        #region Public Methods

        public InfoDataV2 GetInfoDataV2FromDevice() {
            var device = ProgramData.LocalDevice;

            InfoDataV2 infoData = new InfoDataV2();
            infoData.Alias = device.Alias;
            infoData.Version = "2.1";
            infoData.DeviceModel = device.DeviceModel;
            infoData.DeviceType = device.DeviceType;
            infoData.FingerPrint = device.Fingerprint;
            infoData.Port = device.Port;
            infoData.Protocol = device.ProtocolVersion;
            infoData.Download = false;
            return infoData;
        }


        #endregion Public Methods

    }
}