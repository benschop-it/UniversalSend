using System.Collections.Generic;
using System.Diagnostics;
using UniversalSend.Models;
using UniversalSend.Models.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls {

    public sealed partial class DeviceInfoControl : UserControl {

        #region Private Fields

        private string DeviceName = ProgramData.LocalDevice.Alias;
        private string IP = ProgramData.LocalDevice.IP;
        private int Port = ProgramData.LocalDevice.Port;

        #endregion Private Fields

        #region Public Constructors

        public DeviceInfoControl() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            IP = "";
            List<string> ipList = NetworkHelper.GetIPv4AddrList();
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