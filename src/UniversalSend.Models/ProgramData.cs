using System;
using UniversalSend.Models.Data;

namespace UniversalSend.Models {

    public class ProgramData {

        #region Public Properties

        public static Device LocalDevice { get; set; } = new Device {
            //Alias = $"WindowsPhone (UWP)",
            ProtocolVersion = "v1",
            //DeviceModel = "Microsoft",
            //DeviceType = "Desktop",
            Fingerprint = Guid.NewGuid().ToString(),
            //Port = 53317,
            HttpProtocol = "http",
        };

        #endregion Public Properties
    }
}