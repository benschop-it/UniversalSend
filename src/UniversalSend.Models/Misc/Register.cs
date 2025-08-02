using System;
using UniversalSend.Models.Common;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Misc {

    internal class Register : IRegister {

        #region Private Fields

        private readonly IDeviceManager _deviceManager;
        private readonly ILogger _logger;

        #endregion Private Fields

        #region Public Constructors

        public Register(IDeviceManager deviceManager) {
            _logger = LogManager.GetLogger<Register>();
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
        }

        #endregion Public Constructors

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
            _logger.Debug($"Register_NewDeviceRegister {device.Alias} {device.DeviceModel} {device.DeviceType} {device.IP}");
            _deviceManager.AddKnownDevices((Device)sender);
        }

        #endregion Private Methods

    }
}