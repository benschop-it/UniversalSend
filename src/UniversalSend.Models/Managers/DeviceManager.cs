using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Interfaces.Internal;

namespace UniversalSend.Models.Managers {

    internal class DeviceManager : IDeviceManager {

        #region Private Fields

        private IHttpClientHelper _httpClientHelper;
        private INetworkHelper _networkHelper;
        private IRegisterRequestDataManager _registerRequestDataManager;

        #endregion Private Fields

        #region Public Constructors

        public DeviceManager(
            INetworkHelper networkHelper,
            IRegisterRequestDataManager registerRequestDataManager,
            IHttpClientHelper httpClientHelper
        ) {
            _networkHelper = networkHelper ?? throw new ArgumentNullException(nameof(networkHelper));
            _httpClientHelper = httpClientHelper ?? throw new ArgumentNullException(nameof(httpClientHelper));
            _registerRequestDataManager = registerRequestDataManager ?? throw new ArgumentNullException(nameof(registerRequestDataManager));
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
            if (
                KnownDevices.Find(x => x.Fingerprint == device.Fingerprint) != null ||
                ProgramData.LocalDevice.Fingerprint == device.Fingerprint
            ) {
                return;
            }

            KnownDevices.Add(device);
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
            };
            return device;
        }

        public IDevice CreateDeviceFromInfoData(IInfoData info) {
            return new Device {
                Alias = info.Alias,
                DeviceModel = info.DeviceModel,
                DeviceType = info.DeviceType
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

            Debug.WriteLine($"URL:http://{IP}:53317/api/localsend/v1/register\nJson:{serializedDevice}");

            string responseString = await _httpClientHelper.PostJsonAsync($"http://{IP}:53317/api/localsend/v1/register", serializedDevice);

            Debug.WriteLine($"responseString: {responseString}");

            try {
                RegisterResponseData registerResponseData = JsonConvert.DeserializeObject<RegisterResponseData>(responseString);
                if (registerResponseData == null) {
                    return null;
                }

                IDevice device = new Device();
                device.IP = IP;
                device.Port = 53317;
                device.Alias = registerResponseData.Alias;
                device.DeviceModel = registerResponseData.DeviceModel;
                device.DeviceType = registerResponseData.DeviceType;
                device.Fingerprint = registerResponseData.Fingerprint;
                return device;
            } catch {
            }
            return null;
        }

        public IDevice GetDeviceFromRequestData(IRegisterRequestData registerRequestData, string ip, int port) {
            IDevice device = new Device {
                Alias = registerRequestData.Alias,
                HttpProtocol = "http",
                ProtocolVersion = "v1",
                DeviceModel = registerRequestData.DeviceModel,
                DeviceType = registerRequestData.DeviceType,
                Fingerprint = registerRequestData.Fingerprint,
                IP = ip,
                Port = port
            };
            return device;
        }

        public IDevice GetDeviceFromResponseData(IRegisterResponseData responseData, string ip) {
            IDevice device = new Device();
            device.IP = ip;
            device.Port = ProgramData.LocalDevice.Port;
            device.Alias = responseData.Alias;
            device.DeviceModel = responseData.DeviceModel;
            device.DeviceType = responseData.DeviceType;
            device.Fingerprint = responseData.Fingerprint;

            return device;
        }

        public IDevice GetLocalDevice() {
            IDevice localDevice = new Device {
                //Alias = $"WindowsPhone (UWP)",
                ProtocolVersion = "v1",
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

        private async Task SearchKnownDeviceAsync(string ip) {
            IDevice device = await FindDeviceByIPAsync(ip);
            if (device != null) {
                AddKnownDevices(device);
            }
        }

        #endregion Private Methods
    }
}