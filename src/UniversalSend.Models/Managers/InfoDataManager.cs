using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {
    internal class InfoDataManager : IInfoDataManager {

        #region Public Methods

        public InfoData GetInfoDataFromDevice() {
            var device = ProgramData.LocalDevice;

            InfoData infoData = new InfoData();
            infoData.Alias = device.Alias;
            infoData.DeviceType = device.DeviceType;
            infoData.DeviceModel = device.DeviceModel;
            return infoData;
        }

        #endregion Public Methods
    }
}