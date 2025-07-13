﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Data
{
    public class Device
    {
        public string Alias { get; set; } = ""; // Device alias
        public string HttpProtocol { get; set; } = "http"; // Protocol
        public string ProtocolVersion { get; set; } = ""; // LocalSend protocol version v1 | v2
        public string DeviceModel { get; set; } = ""; // Device brand
        public string DeviceType { get; set; } = ""; // Device type: mobile | desktop | web | headless | server
        public string Fingerprint { get; set; } = ""; // Device identifier fingerprint

        public string IP { get; set; } = ""; // IP address
        public int Port { get; set; } = -1; // Port number
    }

    public class DeviceManager
    {
        public static event EventHandler KnownDevicesChanged;
        public static List<Device> KnownDevices { get; set; } = new List<Device>();

        public static void AddKnownDevices(Device device)
        {
            if (KnownDevices.Find(x => x.Fingerprint == device.Fingerprint) != null || ProgramData.LocalDevice.Fingerprint == device.Fingerprint)
                return;
            KnownDevices.Add(device);
            KnownDevicesChanged?.Invoke(null, EventArgs.Empty);
        }

        public static async Task SearchKnownDevicesAsync(List<string> ipList)
        {
            foreach (var ip in ipList)
                await SearchKnownDeviceAsync(ip);
        }

        public static async Task SearchKnownDevicesAsync()
        {
            List<string> localIPList = NetworkHelper.GetIPv4AddrList();

            for (int i = 0; i < localIPList.Count; i++)
            {
                localIPList[i] = localIPList[i].Substring(0, localIPList[i].LastIndexOf(".") + 1);
                for (int j = 133; j < 134; j++)
                {
                    await SearchKnownDeviceAsync(localIPList[i] + j);
                }
            }
        }

        static async Task SearchKnownDeviceAsync(string ip)
        {
            Device device = await FindDeviceByIPAsync(ip);
            if (device != null)
                AddKnownDevices(device);
        }

        public static void ClearKnownDevices()
        {
            KnownDevices.Clear();
        }

        public static async Task<Device> FindDeviceByIPAsync(string IP)
        {
            Debug.WriteLine($"URL:http://{IP}:53317/api/localsend/v1/register\nJson:{JsonConvert.SerializeObject(RegisterRequestDataManager.CreateFromDevice(ProgramData.LocalDevice))}");
            string responseString = await HttpClientHelper.PostJsonAsync($"http://{IP}:53317/api/localsend/v1/register", JsonConvert.SerializeObject(RegisterRequestDataManager.CreateFromDevice(ProgramData.LocalDevice)));

            try
            {
                RegisterResponseData registerResponseData = JsonConvert.DeserializeObject<RegisterResponseData>(responseString);
                if (registerResponseData == null)
                    return null;
                Device device = new Device();
                device.IP = IP;
                device.Port = 53317;
                device.Alias = registerResponseData.alias;
                device.DeviceModel = registerResponseData.deviceModel;
                device.DeviceType = registerResponseData.deviceType;
                device.Fingerprint = registerResponseData.fingerprint;
                return device;
            }
            catch
            {
            }
            return null;
        }

        public static async Task<Device> FindDeviceByHashTagAsync(string HashTag)
        {
            List<string> localIPList = NetworkHelper.GetIPv4AddrList();
            Device device = null;
            for (int i = 0; i < localIPList.Count; i++)
            {
                localIPList[i] = localIPList[i].Substring(0, localIPList[i].LastIndexOf(".") + 1);
                device = await FindDeviceByIPAsync(localIPList[i] + HashTag);
                if (device != null)
                    return device;
            }
            return null;
        }

        public static Device CreateDeviceFromInfoData(InfoData info)
        {
            return new Device { Alias = info.alias, DeviceModel = info.deviceModel, DeviceType = info.deviceType };
        }

        public enum DeviceType
        {
            mobile,
            desktop,
            web,
            headless,
            server
        }
    }
}
