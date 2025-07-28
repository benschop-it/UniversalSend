using Windows.System.Profile;

namespace UniversalSend.Models.Helpers {

    public class SystemHelper {

        public enum DeviceFormFactorType {
            Phone,
            Desktop,
            Tablet,
            IoT,
            SurfaceHub,
            Xbox,
            Holographic,
            Other
        }

        public static DeviceFormFactorType GetDeviceFormFactorType() {
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
    }
}