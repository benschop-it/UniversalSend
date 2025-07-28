using System;
using System.Diagnostics;
using UniversalSend.Models.Data;

namespace UniversalSend.Models {

    public class Register {

        #region Public Events

        public static event EventHandler NewDeviceRegister;

        #endregion Public Events

        #region Public Methods

        public static void NewDeviceRegisterV1Event(Device device) {
            NewDeviceRegister?.Invoke(device, EventArgs.Empty);
        }

        public static void StartRegister() {
            NewDeviceRegister += Register_NewDeviceRegister;
        }

        #endregion Public Methods

        #region Private Methods

        private static void Register_NewDeviceRegister(object sender, EventArgs e) {
            Device device = sender as Device;
            if (device == null) {
                return;
            }
            Debug.WriteLine($"Register_NewDeviceRegister {device.Alias} {device.DeviceModel} {device.DeviceType} {device.IP}");
            DeviceManager.AddKnownDevices((Device)sender);
        }

        #endregion Private Methods
    }
}