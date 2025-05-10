using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalSend.Controls;
using UniversalSend.Controls.SettingControls;
using UniversalSend.Models;
using UniversalSend.Models.Data;
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
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        void InitControls()
        {
            InitNetworkControls();
        }

        void InitNetworkControls()
        {
            Dictionary<int, string> selectionDisplayName = new Dictionary<int, string>();
            selectionDisplayName.Add((int)DeviceManager.DeviceType.mobile, "手机/平板");
            selectionDisplayName.Add((int)DeviceManager.DeviceType.desktop, "电脑");
            selectionDisplayName.Add((int)DeviceManager.DeviceType.web, "网页");
            selectionDisplayName.Add((int)DeviceManager.DeviceType.headless, "终端");
            selectionDisplayName.Add((int)DeviceManager.DeviceType.server, "服务器");

            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("别名",new TextSettingControl(Settings.Network_DeviceName)));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("设备类型",new ComboSettingsControl(Settings.Network_DeviceType,typeof(DeviceManager.DeviceType),selectionDisplayName)));//替换为下拉框
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("设备型号",new TextSettingControl(Settings.Network_DeviceModel)));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("端口",new NumberSettingControl(Settings.Network_Port)));//替换为NumberBox
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("多线程广播",new TextSettingControl(Settings.Network_MulticaastAddress)));
        }

        private void NavigateToDevPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DevPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitControls();
        }
    }
}
