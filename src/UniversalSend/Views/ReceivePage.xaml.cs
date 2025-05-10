using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UniversalSend.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ReceivePage : Page
    {
        public ReceivePage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AliasTextBlock.Text = Settings.GetSettingContentAsString(Settings.Network_DeviceName);

            List<string>IpAddrList = NetworkHelper.GetIPv4AddrList();
            foreach(string ip in IpAddrList)
            {
                //Debug.WriteLine(ip);
                HashtagTextBlock.Text += $"#{ip.Substring(ip.LastIndexOf(".")+1)} ";
            }
        }
    }
}
