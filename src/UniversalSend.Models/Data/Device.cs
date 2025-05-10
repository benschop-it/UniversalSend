using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
