using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.HttpData
{
    public class InfoData
    {
        public string alias { get; set; }
        public string deviceModel { get; set; }
        public string deviceType { get; set; }
    }

    public class InfoDataManager
    {
        public static InfoData GetInfoDataFromDevice(Device device)
        {
            InfoData infoData = new InfoData();
            infoData.alias = device.Alias;
            infoData.deviceType = device.DeviceType;
            infoData.deviceModel = device.DeviceModel;
            return infoData;
        }
    }
}
