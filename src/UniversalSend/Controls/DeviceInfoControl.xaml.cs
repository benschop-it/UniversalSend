using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalSend.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace UniversalSend.Controls
{
    public sealed partial class DeviceInfoControl : UserControl
    {
        string DeviceName = ProgramData.LocalDevice.Alias;
        string IP = ProgramData.LocalDevice.IP;
        int Port = ProgramData.LocalDevice.Port;

        public DeviceInfoControl()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            IP = "";
            List<string> ipList = NetworkHelper.GetIPv4AddrList();
            foreach (string ip in ipList)
            {
                IP += $"{ip}\n";
            }
            if (IP.Length > 1)
                IP = IP.Substring(0, IP.Length - 1);
            Bindings.Update();
        }
    }
}
