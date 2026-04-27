using System;
using UniversalSend.Models.Common;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;

namespace UniversalSend.Models.Misc {

    internal class Settings : ISettings {

        #region Public Fields

        // Use internal file manager
        public ApplicationDataContainer UserSettings;

        #endregion Public Fields

        #region Private Fields

        private readonly ILogger _logger;
        private ISystemHelper _systemHelper;

        #endregion Private Fields

        #region Public Constructors

        public Settings(ISystemHelper systemHelper) {
            _logger = LogManager.GetLogger<Settings>();
            _systemHelper = systemHelper ?? throw new ArgumentNullException(nameof(systemHelper));
        }

        #endregion Public Constructors

        #region Public Methods

        public object GetSettingContent(string key) {
            object value;
            if (UserSettings.Values.TryGetValue(key, out value)) {
                return value;
            } else {
                return null;
            }
        }

        public string GetSettingContentAsString(string key) {
            object value;
            if (UserSettings.Values.TryGetValue(key, out value)) {
                return value.ToString();
            } else {
                return null;
            }
        }

        public void InitUserSettings() {
            UserSettings = ApplicationData.Current.LocalSettings;
            InitSettings();
        }

        public bool SetInitSetting(string key, object value) {
            if (UserSettings.Values.ContainsKey(key)) {
                return false;
            }

            UserSettings.Values.Add(key, value);
            return true;
        }

        public bool SetSetting(string key, object value) {
            try {
                UserSettings.Values[key] = value;
            } catch (Exception ex) {
                _logger.Error("Exception storing settings.", ex);
            }
            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private void InitSettings() {
            // Device name
            EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
            SetInitSetting(Constants.Network_DeviceName, $"{deviceInfo.SystemProductName} (UWP)");

            // Device type
            DeviceFormFactorType deviceType = _systemHelper.GetDeviceFormFactorType();
            switch (deviceType) {
                case DeviceFormFactorType.Phone:
                    SetInitSetting(Constants.Network_DeviceType, "mobile");
                    break;
                case DeviceFormFactorType.Desktop:
                case DeviceFormFactorType.SurfaceHub:
                case DeviceFormFactorType.Tablet:
                    SetInitSetting(Constants.Network_DeviceType, "desktop");
                    break;
                case DeviceFormFactorType.IoT:
                    SetInitSetting(Constants.Network_DeviceType, "headless");
                    break;
                case DeviceFormFactorType.Xbox:
                    SetInitSetting(Constants.Network_DeviceType, "server");
                    break;
                default:
                    SetInitSetting(Constants.Network_DeviceType, "web");
                    break;
            }

            // Device model
            if (!string.IsNullOrEmpty(deviceInfo.SystemSku)) {
                SetInitSetting(Constants.Network_DeviceModel, deviceInfo.SystemSku);
            } else {
                SetInitSetting(Constants.Network_DeviceModel, deviceInfo.SystemProductName);
            }

            // Fingerprint
            SetInitSetting(Constants.Network_Fingerprint, Guid.NewGuid().ToString("N"));

            // Port
            SetInitSetting(Constants.Network_Port, 53317);

            // Multicast address
            SetInitSetting(Constants.Network_MulticastAddress, "224.0.0.167");

            // Lab - Use internal file manager
            SetInitSetting(Constants.Lab_UseInternalExplorer, "False");

            // Web share - Require PIN (default: on)
            SetInitSetting(Constants.WebShare_RequirePin, true);

            // Web share - PIN (default: random 6-digit)
            SetInitSetting(Constants.WebShare_Pin, new Random().Next(100000, 999999).ToString());
        }

        #endregion Private Methods

    }
}