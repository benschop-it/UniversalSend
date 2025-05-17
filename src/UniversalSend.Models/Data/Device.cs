using Newtonsoft.Json;
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
        public string Alias { get; set; } = "";//设备别名
        public string HttpProtocol { get; set; } = "http";//协议
        public string ProtocolVersion { get; set; } = "";//LocalSend协议版本 v1 | v2
        public string DeviceModel { get; set; } = "";//设备品牌
        public string DeviceType { get; set; } = "";//设备类型 mobile | desktop | web | headless | server
        public string Fingerprint { get; set; } = "";//设备标识指纹

        public string IP { get; set; } = "";//IP地址
        public int Port { get; set; } = -1;//端口号
    }

    public class DeviceManager
    {
        public static List<Device> KnownDevices { get; set; } = new List<Device>();

        public static void AddKnownDevices(Device device)
        {
            if (KnownDevices.Find(x => x.Fingerprint == device.Fingerprint) != null || ProgramData.LocalDevice.Fingerprint == device.Fingerprint)
                return;
            KnownDevices.Add(device);
        }

        public static async Task SearchKnownDevicesAsync()
        {
            KnownDevices.Clear();
            List<string> localIPList = NetworkHelper.GetIPv4AddrList();
            Device device = null;
            for (int i = 0; i < localIPList.Count; i++)
            {
                localIPList[i] = localIPList[i].Substring(0, localIPList[i].LastIndexOf(".") + 1);
                for (int j = 0; j < 256; j++)
                {
                    device = await FindDeviceByIPAsync(localIPList[i] + j);
                    if (device != null)
                        AddKnownDevices(device);
                }
            }
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
            /*To-Do:完善通过标签获取的功能*/
            List<string> localIPList = NetworkHelper.GetIPv4AddrList();
            Device device = null;
            for (int i=0;i<localIPList.Count;i++)
            {
                localIPList[i] = localIPList[i].Substring(0, localIPList[i].LastIndexOf(".")+1);
                for(int j = 0; j < 256; j++)
                {
                    device = await FindDeviceByIPAsync(localIPList[i]+j);
                    if (device != null)
                        return device;
                }
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
