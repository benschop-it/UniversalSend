using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalSend.Models;
using UniversalSend.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UniversalSend
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Settings.InitUserSettings();
            InitData();
            string deviceFamilyVersion = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong build = (version & 0x00000000FFFF0000L) >> 16;
            Debug.WriteLine($"系统版本Build号：{build}");
            if (build >= 16299)
            {
                Frame.Navigate(typeof(RootPage));
            }else
            {
                Frame.Navigate(typeof(RootPage15063));
            }
            
        }

        void InitData()
        {
            ProgramData.LocalDevice.Alias = Settings.GetSettingContentAsString(Settings.Network_DeviceName);
            ProgramData.LocalDevice.DeviceModel = Settings.GetSettingContentAsString(Settings.Network_DeviceModel);
            ProgramData.LocalDevice.DeviceType = Settings.GetSettingContentAsString(Settings.Network_DeviceType);
            ProgramData.LocalDevice.Port = (int)Settings.GetSettingContent(Settings.Network_Port);
        }
    }
}
