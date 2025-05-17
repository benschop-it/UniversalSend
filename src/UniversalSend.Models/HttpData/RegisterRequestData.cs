using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.HttpData
{
    public class RegisterRequestData
    {
        public string alias { get; set; }
        public string deviceModel { get; set; }
        public string deviceType { get; set; }
        public string fingerprint { get; set; }
    }

    public class RegisterRequestDataManager
    {
        public static RegisterRequestData CreateFromDevice(Device device)
        {
            RegisterRequestData registerRequestData = new RegisterRequestData();
            registerRequestData.alias = device.Alias;
            registerRequestData.deviceModel = device.DeviceModel;
            registerRequestData.deviceType = device.DeviceType;
            registerRequestData.fingerprint = device.Fingerprint;
            return registerRequestData;
        }
    }
}
