using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.HttpData
{
    public sealed class RegisterData
    {
        public string alias { get; set; }
        public string version { get; set; }
        public string deviceModel { get; set; }
        public string deviceType { get; set; }
        public string fingerprint { get; set; }
        public int port { get; set; }
        public string protocol { get; set; }
        public bool download { get; set; }
        public bool announce { get; set; }
    }

    public class RegisterDataManager
    {
        /// <summary>
        /// Gets RegisterData from a Device instance, commonly used to retrieve local device registration information
        /// </summary>
        public static RegisterData GetRegisterDataFromDevice(Device device)
        {
            RegisterData registerData = new RegisterData();
            registerData.alias = device.Alias;
            registerData.version = device.ProtocolVersion;
            registerData.deviceModel = device.DeviceModel;
            registerData.deviceType = device.DeviceType;
            registerData.fingerprint = device.Fingerprint;
            registerData.port = device.Port;
            registerData.protocol = device.HttpProtocol;
            registerData.download = true;
            registerData.announce = false;
            return registerData;
        }
    }
}
