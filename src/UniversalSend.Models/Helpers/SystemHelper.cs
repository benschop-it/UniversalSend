using UniversalSend.Models.Interfaces;
using Windows.System.Profile;

namespace UniversalSend.Models.Helpers {

    internal class SystemHelper : ISystemHelper {

        #region Public Methods

        public DeviceFormFactorType GetDeviceFormFactorType() {
            switch (AnalyticsInfo.VersionInfo.DeviceFamily) {
                case "Windows.Mobile":
                    return DeviceFormFactorType.Phone;
                case "Windows.Desktop":
                    return DeviceFormFactorType.Desktop;
                case "Windows.IoT":
                    return DeviceFormFactorType.IoT;
                case "Windows.Team":
                    return DeviceFormFactorType.SurfaceHub;
                case "Windows.Xbox":
                    return DeviceFormFactorType.Xbox;
                case "Windows.Holographic":
                    return DeviceFormFactorType.Holographic;
                default:
                    return DeviceFormFactorType.Other;
            }
        }

        #endregion Public Methods
    }
}