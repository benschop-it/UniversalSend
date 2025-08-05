using System;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models {

    public class ProgramData {

        #region Public Properties

        public static IDevice LocalDevice { get; set; } = new Device {
            //Alias = $"WindowsPhone (UWP)",
            ProtocolVersion = "2.1",
            //DeviceModel = "Microsoft",
            //DeviceType = "Desktop",
            Fingerprint = Guid.NewGuid().ToString(),
            //Port = 53317,
            HttpProtocol = "http",
        };

        #endregion Public Properties
    }
}