using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Controls;
using UniversalSend.Controls.SettingControls;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using Windows.ApplicationModel;
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
            PageHeader.Margin = UIManager.RootElementMargin;
            RootStackPanel.Margin = UIManager.RootElementMarginWithoutTop;
        }

        void InitControls()
        {
            InitNetworkControls();
            InitReceiveControls();
            InitAboutControls();
            InitLabControls();
        }

        void InitReceiveControls()
        {
            ReceiveSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("SettingsPage_Receive_SaveToFolder_Header")/*"保存目录"*/, new SaveLocationSettingControl()));
        }

        void InitNetworkControls()
        {
            Dictionary<int, string> selectionDisplayName = new Dictionary<int, string>();
            selectionDisplayName.Add((int)DeviceManager.DeviceType.mobile, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Mobile") /*"手机/平板"*/);
            selectionDisplayName.Add((int)DeviceManager.DeviceType.desktop, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Desktop")/*"电脑"*/);
            selectionDisplayName.Add((int)DeviceManager.DeviceType.web, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Web")/*"网页"*/);
            selectionDisplayName.Add((int)DeviceManager.DeviceType.headless, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Headless")/*"终端"*/);
            selectionDisplayName.Add((int)DeviceManager.DeviceType.server, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Server")/*"服务器"*/);

            

            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_Server")/*"服务器"*/, new ServerManageControl()));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_DeviceName")/*"别名"*/, new TextSettingControl(Settings.Network_DeviceName)));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_DeviceType")/*"设备类型"*/, new ComboSettingsControl(Settings.Network_DeviceType,typeof(DeviceManager.DeviceType),selectionDisplayName)));//替换为下拉框
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_DeviceModel")/*"设备型号"*/, new TextSettingControl(Settings.Network_DeviceModel)));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_Port")/*"端口"*/, new NumberSettingControl(Settings.Network_Port)));//替换为NumberBox
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_MulticastAddress")/*"多线程广播"*/, new TextSettingControl(Settings.Network_MulticastAddress)));
        }

        void InitLabControls()
        {
            LabSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("SettingsPage_Lab_UseInternalExplorer")/*使用内部文件管理器*/,new ToggleSwitchSettingsControl(Settings.Lab_UseInternalExplorer)));
        }

        void InitAboutControls()
        {
            

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
