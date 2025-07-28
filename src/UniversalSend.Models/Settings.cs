using UniversalSend.Models.Helpers;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;

namespace UniversalSend.Models {

    public class Settings {

        #region Public Fields

        public const string ExplorerPage_ViewMode = "ExplorerPage_ViewMode";
        public const string Favorite_Favorites = "Favorite_Favorites";
        public const string Lab_UseInternalExplorer = "Lab_UseInternalExplorer";
        public const string Network_DeviceModel = "Network_DeviceModel";
        public const string Network_DeviceName = "Network_DeviceName";
        public const string Network_DeviceType = "Network_DeviceType";
        public const string Network_MulticastAddress = "Network_MulticastAddress";
        public const string Network_Port = "Network_Port";
        public const string Receive_Histories = "Receive_Histories";
        public const string Receive_SaveToFolder = "Receive_SaveToFolder";

        // Use internal file manager
        public static ApplicationDataContainer UserSettings;

        #endregion Public Fields

        #region Public Methods

        public static object GetSettingContent(string key) {
            object value;
            if (UserSettings.Values.TryGetValue(key, out value)) {
                return value;
            } else {
                return null;
            }
        }

        public static string GetSettingContentAsString(string key) {
            object value;
            if (UserSettings.Values.TryGetValue(key, out value)) {
                return value.ToString();
            } else {
                return null;
            }
        }

        public static void InitUserSettings() {
            UserSettings = ApplicationData.Current.LocalSettings;
            InitSettings();
        }

        public static bool SetInitSetting(string key, object value) {
            if (UserSettings.Values.ContainsKey(key)) {
                return false;
            }

            UserSettings.Values.Add(key, value);
            return true;
        }

        public static bool SetSetting(string key, object value) {
            UserSettings.Values[key] = value;
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private static void InitSettings() {
            // Device name
            EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
            SetInitSetting(Network_DeviceName, $"{deviceInfo.SystemProductName} (UWP)");

            // Device type
            SystemHelper.DeviceFormFactorType deviceType = SystemHelper.GetDeviceFormFactorType();
            switch (deviceType) {
                case SystemHelper.DeviceFormFactorType.Phone:
                    SetInitSetting(Network_DeviceType, "mobile");
                    break;
                case SystemHelper.DeviceFormFactorType.Desktop:
                case SystemHelper.DeviceFormFactorType.SurfaceHub:
                case SystemHelper.DeviceFormFactorType.Tablet:
                    SetInitSetting(Network_DeviceType, "desktop");
                    break;
                case SystemHelper.DeviceFormFactorType.IoT:
                    SetInitSetting(Network_DeviceType, "headless");
                    break;
                case SystemHelper.DeviceFormFactorType.Xbox:
                    SetInitSetting(Network_DeviceType, "server");
                    break;
                default:
                    SetInitSetting(Network_DeviceType, "web");
                    break;
            }

            // Device model
            if (!string.IsNullOrEmpty(deviceInfo.SystemSku)) {
                SetInitSetting(Network_DeviceModel, deviceInfo.SystemSku);
            } else {
                SetInitSetting(Network_DeviceModel, deviceInfo.SystemProductName);
            }

            // Port
            SetInitSetting(Network_Port, 53317);

            // Multicast address
            SetInitSetting(Network_MulticastAddress, "224.0.0.167");

            // Lab - Use internal file manager
            SetInitSetting(Lab_UseInternalExplorer, "False");
        }

        #endregion Private Methods
    }
}