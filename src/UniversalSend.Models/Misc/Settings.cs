using System;
using System.IO;
using System.Linq;
using UniversalSend.Models.Common;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;

namespace UniversalSend.Models.Misc {

    internal class Settings : ISettings {

        private const string FileBackedValuePrefix = "@file:";

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
                if (value is string stringValue && TryGetFileBackedValue(stringValue, out var fileBackedValue)) {
                    return fileBackedValue;
                }
                return value;
            } else {
                return null;
            }
        }

        public string GetSettingContentAsString(string key) {
            object value;
            if (UserSettings.Values.TryGetValue(key, out value)) {
                if (value is string stringValue && TryGetFileBackedValue(stringValue, out var fileBackedValue)) {
                    return fileBackedValue;
                }
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
            DeleteFileBackedValueIfPresent(key);

            try {
                UserSettings.Values[key] = value;
            } catch (Exception ex) {
                if (value is string stringValue && TryStoreLargeStringSetting(key, stringValue, ex)) {
                    return true;
                }

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
            SetInitSetting(Constants.WebShare_AutoAccept, false);

            // Web share - Require PIN (default: on)
            SetInitSetting(Constants.WebShare_RequirePin, true);

            // Web share - PIN (default: random 6-digit)
            SetInitSetting(Constants.WebShare_Pin, new Random().Next(100000, 999999).ToString());
        }

        private bool TryStoreLargeStringSetting(string key, string value, Exception originalException) {
            try {
                string fileName = GetFileBackedSettingFileName(key);
                File.WriteAllText(Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName), value);
                UserSettings.Values[key] = FileBackedValuePrefix + fileName;
                _logger.Warn($"Stored oversized setting '{key}' as file-backed value.");
                return true;
            } catch (Exception fileException) {
                _logger.Error($"Failed to persist oversized setting '{key}' as file-backed value.", fileException);
                _logger.Error("Original exception storing settings.", originalException);
                return false;
            }
        }

        private bool TryGetFileBackedValue(string rawValue, out string value) {
            value = null;

            if (string.IsNullOrWhiteSpace(rawValue) || !rawValue.StartsWith(FileBackedValuePrefix, StringComparison.Ordinal)) {
                return false;
            }

            string fileName = rawValue.Substring(FileBackedValuePrefix.Length);
            string fullPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName);
            if (!File.Exists(fullPath)) {
                return false;
            }

            value = File.ReadAllText(fullPath);
            return true;
        }

        private void DeleteFileBackedValueIfPresent(string key) {
            if (UserSettings == null) {
                return;
            }

            object value;
            if (!UserSettings.Values.TryGetValue(key, out value)) {
                return;
            }

            if (!(value is string stringValue) || !stringValue.StartsWith(FileBackedValuePrefix, StringComparison.Ordinal)) {
                return;
            }

            string fileName = stringValue.Substring(FileBackedValuePrefix.Length);
            string fullPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName);
            try {
                if (File.Exists(fullPath)) {
                    File.Delete(fullPath);
                }
            } catch (Exception ex) {
                _logger.Warn($"Failed to delete file-backed setting storage for '{key}'.", ex);
            }
        }

        private static string GetFileBackedSettingFileName(string key) {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = new string((key ?? string.Empty)
                .Select(c => invalidChars.Contains(c) ? '_' : c)
                .ToArray());
            return $"setting_{sanitized}.json";
        }

        #endregion Private Methods

    }
}