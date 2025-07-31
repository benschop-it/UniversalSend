using System;
using System.Diagnostics;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models {

    public class Register : IRegister {

        private readonly IDeviceManager _deviceManager;

        public Register(IDeviceManager deviceManager) {
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
        }

        #region Public Events

        public event EventHandler NewDeviceRegister;

        #endregion Public Events

        #region Public Methods

        public void NewDeviceRegisterV1Event(IDevice device) {
            NewDeviceRegister?.Invoke(device, EventArgs.Empty);
        }

        public void StartRegister() {
            NewDeviceRegister += Register_NewDeviceRegister;
        }

        #endregion Public Methods

        #region Private Methods

        private void Register_NewDeviceRegister(object sender, EventArgs e) {
            IDevice device = sender as IDevice;
            if (device == null) {
                return;
            }
            Debug.WriteLine($"Register_NewDeviceRegister {device.Alias} {device.DeviceModel} {device.DeviceType} {device.IP}");
            _deviceManager.AddKnownDevices((Device)sender);
        }

        #endregion Private Methods
    }
}