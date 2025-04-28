using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.RestupModels;

namespace UniversalSend.Models
{
    public class ProgramData
    {
        public static RegisterData LocalDeviceRegisterData { get; set; } = new RegisterData
        {
            alias = $"{Environment.MachineName} (UWP)",
            version = "v2",
            deviceModel = "Microsoft",
            deviceType = "Desktop",
            fingerprint = Guid.NewGuid().ToString(),
            port = 53317,
            protocol = "http",
            download = true,
            announce = false
        };
        public static InfoData LocalDeviceInfoData { get; set; } = new InfoData
        {
            alias = $"{Environment.MachineName} (UWP)",
            deviceModel = "Microsoft",
            deviceType = "Desktop"
        };

        public static ServiceHttpServer ServiceServer { get; set; }
    }
}
