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

// https://go.microsoft.com/fwlink/?LinkId=234238 describes the "Blank Page" item template

namespace UniversalSend.Views
{
    /// <summary>
    /// A blank page that can be used on its own or navigated to within a Frame.
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
            ReceiveSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("SettingsPage_Receive_SaveToFolder_Header")/*"Save directory"*/, new SaveLocationSettingControl()));
        }

        void InitNetworkControls()
        {
            Dictionary<int, string> selectionDisplayName = new Dictionary<int, string>();
            selectionDisplayName.Add((int)DeviceManager.DeviceType.mobile, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Mobile") /*"Phone/Tablet"*/);
            selectionDisplayName.Add((int)DeviceManager.DeviceType.desktop, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Desktop")/*"PC"*/);
            selectionDisplayName.Add((int)DeviceManager.DeviceType.web, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Web")/*"Web"*/);
            selectionDisplayName.Add((int)DeviceManager.DeviceType.headless, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Headless")/*"Terminal"*/);
            selectionDisplayName.Add((int)DeviceManager.DeviceType.server, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Server")/*"Server"*/);

            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_Server")/*"Server"*/, new ServerManageControl()));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_DeviceName")/*"Alias"*/, new TextSettingControl(Settings.Network_DeviceName)));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_DeviceType")/*"Device Type"*/, new ComboSettingsControl(Settings.Network_DeviceType, typeof(DeviceManager.DeviceType), selectionDisplayName))); // replaced with ComboBox
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_DeviceModel")/*"Device Model"*/, new TextSettingControl(Settings.Network_DeviceModel)));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_Port")/*"Port"*/, new NumberSettingControl(Settings.Network_Port))); // replaced with NumberBox
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_MulticastAddress")/*"Multicast Broadcast"*/, new TextSettingControl(Settings.Network_MulticastAddress)));
        }

        void InitLabControls()
        {
            LabSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("SettingsPage_Lab_UseInternalExplorer")/*Use internal file explorer*/, new ToggleSwitchSettingsControl(Settings.Lab_UseInternalExplorer)));
        }

        void InitAboutControls()
        {
            // Placeholder for about section controls
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
