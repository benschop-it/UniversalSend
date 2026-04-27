using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.Common;
using UniversalSend.Models.Data;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Interfaces.Internal;

namespace UniversalSend.Models.Managers {

    internal class DeviceManager : IDeviceManager {

        #region Private Fields

        private readonly ILogger _logger;
        private IHttpClientHelper _httpClientHelper;
        private INetworkHelper _networkHelper;
        private IRegisterRequestDataManager _registerRequestDataManager;
        private IRegisterResponseDataManager _registerResponseDataManager;

        #endregion Private Fields

        #region Public Constructors

        public DeviceManager(
            INetworkHelper networkHelper,
            IRegisterRequestDataManager registerRequestDataManager,
            IHttpClientHelper httpClientHelper,
            IRegisterResponseDataManager registerResponseDataManager
        ) {
            _logger = LogManager.GetLogger<DeviceManager>();
            _networkHelper = networkHelper ?? throw new ArgumentNullException(nameof(networkHelper));
            _httpClientHelper = httpClientHelper ?? throw new ArgumentNullException(nameof(httpClientHelper));
            _registerRequestDataManager = registerRequestDataManager ?? throw new ArgumentNullException(nameof(registerRequestDataManager));
            _registerResponseDataManager = registerResponseDataManager ?? throw new ArgumentNullException(nameof(registerResponseDataManager));
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler KnownDevicesChanged;

        #endregion Public Events

        #region Public Properties

        public List<IDevice> KnownDevices { get; set; } = new List<IDevice>();

        #endregion Public Properties

        #region Public Methods

        public void AddKnownDevices(IDevice device) {
            if (device == null) {
                _logger.Debug("AddKnownDevices skipped because device was null.");
                return;
            }

            var existing = KnownDevices.Find(x => x.Fingerprint == device.Fingerprint);
            if (
                existing != null ||
                ProgramData.LocalDevice.Fingerprint == device.Fingerprint
            ) {
                _logger.Debug("AddKnownDevices skipped alias='{0}', ip='{1}', fingerprint='{2}', existingMatch={3}, localFingerprintMatch={4}.", device.Alias, device.IP, device.Fingerprint, existing != null, ProgramData.LocalDevice.Fingerprint == device.Fingerprint);
                return;
            }

            KnownDevices.Add(device);
            _logger.Debug("AddKnownDevices added alias='{0}', ip='{1}', port={2}, fingerprint='{3}'. TotalKnownDevices={4}.", device.Alias, device.IP, device.Port, device.Fingerprint, KnownDevices.Count);
            KnownDevicesChanged?.Invoke(null, EventArgs.Empty);
        }

        public void ClearKnownDevices() {
            KnownDevices.Clear();
        }

        public IDevice CreateDevice(string alias, string ip, int port) {
            IDevice device = new Device {
                Alias = "RM-1116_15169 (UWP)",
                IP = "192.168.0.193",
                Port = 53317,
                ProtocolVersion = "2.1"
            };
            return device;
        }

        public IDevice CreateDeviceFromInfoDataV2(IInfoDataV2 info) {
            return new Device {
                Alias = info.Alias,
                DeviceModel = info.DeviceModel,
                DeviceType = NormalizeDeviceType(info.DeviceType),
                ProtocolVersion = info.Protocol
            };
        }

        public async Task<IDevice> FindDeviceByHashTagAsync(string HashTag) {
            List<string> localIPList = _networkHelper.GetIPv4AddrList();
            IDevice device = null;
            for (int i = 0; i < localIPList.Count; i++) {
                localIPList[i] = localIPList[i].Substring(0, localIPList[i].LastIndexOf(".") + 1);
                device = await FindDeviceByIPAsync(localIPList[i] + HashTag);
                if (device != null) {
                    return device;
                }
            }
            return null;
        }

        public async Task<IDevice> FindDeviceByIPAsync(string IP) {
            var serializedDevice = JsonConvert.SerializeObject(_registerRequestDataManager.CreateFromDevice(ProgramData.LocalDevice));

            _logger.Debug("FindDeviceByIPAsync sending register request to http://{0}:53317/api/localsend/v2/register with payload: {1}", IP, serializedDevice);

            string responseString = await _httpClientHelper.PostJsonAsync($"http://{IP}:53317/api/localsend/v2/register", serializedDevice);

            _logger.Debug("FindDeviceByIPAsync received response: {0}", responseString);

            try {
                IRegisterResponseDataV2 registerResponseData = _registerResponseDataManager.DeserializeRegisterResponseDataV2(responseString);
                if (registerResponseData != null) {
                    return CreateDeviceFromRegisterResponseDataV2(registerResponseData, IP, 53317);
                }
            } catch (Exception ex) {
                _logger.Debug("FindDeviceByIPAsync failed to deserialize v2 register response.", ex);
            }

            _logger.Debug("FindDeviceByIPAsync falling back to v1 info for {0}.", IP);

            try {
                string infoResponseString = await _httpClientHelper.GetStringAsync($"http://{IP}:53317/api/localsend/v1/info?fingerprint={ProgramData.LocalDevice.Fingerprint}");
                _logger.Debug("FindDeviceByIPAsync received v1 info response: {0}", infoResponseString);

                InfoDataV2 infoData = JsonConvert.DeserializeObject<InfoDataV2>(infoResponseString);
                if (infoData == null) {
                    return null;
                }

                if (infoData.Fingerprint == ProgramData.LocalDevice.Fingerprint) {
                    return null;
                }

                return CreateDeviceFromInfoDataV2(infoData, IP, 53317);
            } catch (Exception ex) {
                _logger.Debug("FindDeviceByIPAsync failed to retrieve legacy v1 info response.", ex);
            }

            return null;
        }

        public IDevice GetDeviceFromRequestDataV2(IRegisterRequestDataV2 registerRequestData, string ip, int port) {
            IDevice device = new Device {
                Alias = registerRequestData.Alias,
                HttpProtocol = registerRequestData.Protocol,
                ProtocolVersion = registerRequestData.Version,
                DeviceModel = registerRequestData.DeviceModel,
                DeviceType = NormalizeDeviceType(registerRequestData.DeviceType),
                Fingerprint = registerRequestData.Fingerprint,
                IP = ip,
                Port = port,
                Version = registerRequestData.Version
            };
            return device;
        }

        public IDevice GetDeviceFromResponseDataV2(IAnnouncementV2 responseData, string ip) {
            IDevice device = new Device();
            device.Alias = responseData.Alias;
            device.Version = responseData.Version;
            device.DeviceModel = responseData.DeviceModel;
            device.DeviceType = NormalizeDeviceType(responseData.DeviceType);
            device.Fingerprint = responseData.Fingerprint;
            device.Port = responseData.Port;
            device.ProtocolVersion = responseData.Version;
            device.HttpProtocol = responseData.Protocol;
            device.IP = ip;
            return device;
        }

        public IDevice GetLocalDevice() {
            IDevice localDevice = new Device {
                //Alias = $"WindowsPhone (UWP)",
                ProtocolVersion = "2.1",
                //DeviceModel = "Microsoft",
                //DeviceType = "Desktop",
                Fingerprint = Guid.NewGuid().ToString(),
                //Port = 53317,
                HttpProtocol = "http",
            };
            return localDevice;
        }

        public async Task SearchKnownDevicesAsync(List<string> ipList) {
            foreach (var ip in ipList) {
                await SearchKnownDeviceAsync(ip);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private IDevice CreateDeviceFromInfoDataV2(IInfoDataV2 infoData, string ip, int port) {
            return new Device {
                Alias = infoData.Alias,
                Version = infoData.Version,
                DeviceModel = infoData.DeviceModel,
                DeviceType = NormalizeDeviceType(infoData.DeviceType),
                Fingerprint = infoData.Fingerprint,
                HttpProtocol = string.IsNullOrWhiteSpace(infoData.Protocol) ? "http" : infoData.Protocol,
                IP = ip,
                Port = infoData.Port > 0 ? infoData.Port : port,
                ProtocolVersion = string.IsNullOrWhiteSpace(infoData.Version) ? "1.0" : infoData.Version,
            };
        }

        private IDevice CreateDeviceFromRegisterResponseDataV2(IRegisterResponseDataV2 registerResponseData, string ip, int port) {
            return new Device {
                Alias = registerResponseData.Alias,
                Version = registerResponseData.Version,
                DeviceModel = registerResponseData.DeviceModel,
                DeviceType = NormalizeDeviceType(registerResponseData.DeviceType),
                Fingerprint = registerResponseData.Fingerprint,
                HttpProtocol = "http",
                IP = ip,
                Port = port,
                ProtocolVersion = registerResponseData.Version,
            };
        }

        private async Task SearchKnownDeviceAsync(string ip) {
            IDevice device = await FindDeviceByIPAsync(ip);
            if (device != null) {
                AddKnownDevices(device);
            }
        }

        /// <summary>
        /// Normalizes a device type string to a known protocol value.
        /// Per the LocalSend protocol spec (section 7.1), unknown values should fall back to "desktop".
        /// Valid values: mobile, desktop, web, headless, server.
        /// </summary>
        private static string NormalizeDeviceType(string deviceType) {
            if (string.IsNullOrWhiteSpace(deviceType)) {
                return "desktop";
            }

            var lower = deviceType.Trim().ToLowerInvariant();
            switch (lower) {
                case "mobile":
                case "desktop":
                case "web":
                case "headless":
                case "server":
                    return lower;
                default:
                    return "desktop";
            }
        }

        #endregion Private Methods
    }
}