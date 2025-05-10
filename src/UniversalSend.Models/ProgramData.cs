using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;

namespace UniversalSend.Models
{
    public class ProgramData
    {
        public static object ServiceServer { get; set; }
        public static Device LocalDevice { get; set; } = new Device 
        {/*{Environment.MachineName}*/
            //Alias = $"WindowsPhone (UWP)",
            ProtocolVersion = "v1",
            //DeviceModel = "Microsoft",
            //DeviceType = "Desktop",
            Fingerprint = Guid.NewGuid().ToString(),
            //Port = 53317,
            HttpProtocol = "http",
        };

        //public static RegisterData LocalDeviceRegisterData { get; set; } = new RegisterData
        //{
        //    alias = $"{Environment.MachineName} (UWP)",
        //    version = "v1",
        //    deviceModel = "Microsoft",
        //    deviceType = "Desktop",
        //    fingerprint = Guid.NewGuid().ToString(),
        //    port = 53317,
        //    protocol = "http",
        //    download = true,
        //    announce = false
        //};

        //public static InfoData LocalDeviceInfoData { get; set; } = new InfoData
        //{
        //    alias = $"{Environment.MachineName} (UWP)",
        //    deviceModel = "Microsoft",
        //    deviceType = "Desktop"
        //};

        //public static ServiceHttpServer ServiceServer { get; set; }
    }
}
