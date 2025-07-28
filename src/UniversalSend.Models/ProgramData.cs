using System;
using UniversalSend.Models.Data;
using UniversalSend.Models.Managers;

namespace UniversalSend.Models {

    public class ProgramData {

        #region Public Properties

        public static ContentDialogManager ContentDialogManager { get; set; } = new ContentDialogManager();

        public static Device LocalDevice { get; set; } = new Device {
            //Alias = $"WindowsPhone (UWP)",
            ProtocolVersion = "v1",
            //DeviceModel = "Microsoft",
            //DeviceType = "Desktop",
            Fingerprint = Guid.NewGuid().ToString(),
            //Port = 53317,
            HttpProtocol = "http",
        };

        public static object ServiceServer { get; set; }

        #endregion Public Properties
    }
}