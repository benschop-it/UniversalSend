using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models
{
    public class Register
    {
        public static event EventHandler NewDeviceRegister;
        public static void NewDeviceRegisterV1Event(Device device)
        {
            NewDeviceRegister?.Invoke(device,EventArgs.Empty);
            
        }

        public static void StartRegister()
        {
            NewDeviceRegister += Register_NewDeviceRegister;
        }

        private static void Register_NewDeviceRegister(object sender, EventArgs e)
        {
            if (!(sender is Device))
                return;
            DeviceManager.AddKnownDevices((Device)sender);
        }
    }
}
