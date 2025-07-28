using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Data {
    public class DeviceManager {

        #region Public Enums

        public enum DeviceType {
            mobile,
            desktop,
            web,
            headless,
            server
        }

        #endregion Public Enums

        #region Public Events

        public static event EventHandler KnownDevicesChanged;

        #endregion Public Events

        #region Public Properties

        public static List<Device> KnownDevices { get; set; } = new List<Device>();

        #endregion Public Properties

        #region Public Methods

        public static void AddKnownDevices(Device device) {
            if (
                KnownDevices.Find(x => x.Fingerprint == device.Fingerprint) != null ||
                ProgramData.LocalDevice.Fingerprint == device.Fingerprint
            ) {
                return;
            }

            KnownDevices.Add(device);
            KnownDevicesChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void ClearKnownDevices() {
            KnownDevices.Clear();
        }

        public static Device CreateDeviceFromInfoData(InfoData info) {
            return new Device {
                Alias = info.Alias,
                DeviceModel = info.DeviceModel,
                DeviceType = info.DeviceType
            };
        }

        public static async Task<Device> FindDeviceByHashTagAsync(string HashTag) {
            List<string> localIPList = NetworkHelper.GetIPv4AddrList();
            Device device = null;
            for (int i = 0; i < localIPList.Count; i++) {
                localIPList[i] = localIPList[i].Substring(0, localIPList[i].LastIndexOf(".") + 1);
                device = await FindDeviceByIPAsync(localIPList[i] + HashTag);
                if (device != null) {
                    return device;
                }
            }
            return null;
        }

        public static async Task<Device> FindDeviceByIPAsync(string IP) {
            Debug.WriteLine($"URL:http://{IP}:53317/api/localsend/v1/register\nJson:{JsonConvert.SerializeObject(RegisterRequestDataManager.CreateFromDevice(ProgramData.LocalDevice))}");

            string responseString = await HttpClientHelper.PostJsonAsync(
                $"http://{IP}:53317/api/localsend/v1/register",
                JsonConvert.SerializeObject(RegisterRequestDataManager.CreateFromDevice(ProgramData.LocalDevice))
            );

            try {
                RegisterResponseData registerResponseData = JsonConvert.DeserializeObject<RegisterResponseData>(responseString);
                if (registerResponseData == null) {
                    return null;
                }

                Device device = new Device();
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

        public static async Task SearchKnownDevicesAsync(List<string> ipList) {
            foreach (var ip in ipList)
                await SearchKnownDeviceAsync(ip);
        }

        public static async Task SearchKnownDevicesAsync() {
            List<string> localIPList = NetworkHelper.GetIPv4AddrList();

            for (int i = 0; i < localIPList.Count; i++) {
                localIPList[i] = localIPList[i].Substring(0, localIPList[i].LastIndexOf(".") + 1);
                for (int j = 10; j < 11; j++) {
                    await SearchKnownDeviceAsync(localIPList[i] + j);
                }
                for (int j = 133; j < 134; j++) {
                    await SearchKnownDeviceAsync(localIPList[i] + j);
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static async Task SearchKnownDeviceAsync(string ip) {
            Device device = await FindDeviceByIPAsync(ip);
            if (device != null)
                AddKnownDevices(device);
        }

        #endregion Private Methods
    }
}