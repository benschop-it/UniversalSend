using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Diagnostics;
using UniversalSend.Models;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls {

    public sealed partial class DeviceInfoControl : UserControl {

        private IDeviceManager _deviceManager => App.Services.GetRequiredService<IDeviceManager>();

        #region Private Fields

        private string DeviceName;
        private string IP;
        private int Port;
        private INetworkHelper _networkHelper => App.Services.GetRequiredService<INetworkHelper>();

        #endregion Private Fields

        #region Public Constructors

        public DeviceInfoControl() {
            IDevice localDevice = _deviceManager.GetLocalDevice();
            DeviceName = localDevice.Alias;
            IP = localDevice.IP;
            Port = localDevice.Port;
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            var localDevice = ProgramData.LocalDevice;

            Port = localDevice.Port;
            DeviceName = localDevice.Alias;

            IP = "";
            List<string> ipList = _networkHelper.GetIPv4AddrList();
            foreach (string ip in ipList) {
                IP += $"{ip}\n";
            }

            Debug.WriteLine("IPv4 addresses: " + IP);

            if (IP.Length > 1) {
                IP = IP.Substring(0, IP.Length - 1);
            }

            Bindings.Update();
        }

        #endregion Private Methods
    }
}