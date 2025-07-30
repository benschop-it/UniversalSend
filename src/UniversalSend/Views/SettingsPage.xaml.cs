using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using UniversalSend.Controls;
using UniversalSend.Controls.SettingControls;
using UniversalSend.Interfaces;
using UniversalSend.Misc;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Views {

    public sealed partial class SettingsPage : Page {

        private IUIManager _uiManager => App.Services.GetRequiredService<IUIManager>();

        #region Public Constructors

        public SettingsPage() {
            InitializeComponent();
            PageHeader.Margin = _uiManager.RootElementMargin;
            RootStackPanel.Margin = _uiManager.RootElementMarginWithoutTop;
        }

        #endregion Public Constructors

        #region Private Methods

        private void InitAboutControls() {
            // Placeholder for about section controls
        }

        private void InitControls() {
            InitNetworkControls();
            InitReceiveControls();
            InitAboutControls();
            InitLabControls();
        }

        private void InitLabControls() {
            LabSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("SettingsPage_Lab_UseInternalExplorer")/*Use internal file explorer*/, new ToggleSwitchSettingsControl(Constants.Lab_UseInternalExplorer)));
        }

        private void InitNetworkControls() {
            Dictionary<int, string> selectionDisplayName = new Dictionary<int, string>();
            selectionDisplayName.Add((int)DeviceType.mobile, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Mobile") /*"Phone/Tablet"*/);
            selectionDisplayName.Add((int)DeviceType.desktop, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Desktop")/*"PC"*/);
            selectionDisplayName.Add((int)DeviceType.web, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Web")/*"Web"*/);
            selectionDisplayName.Add((int)DeviceType.headless, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Headless")/*"Terminal"*/);
            selectionDisplayName.Add((int)DeviceType.server, LocalizeManager.GetLocalizedString("SettingsPage_Network_DeviceType_Server")/*"Server"*/);

            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_Server")/*"Server"*/, new ServerManageControl()));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_DeviceName")/*"Alias"*/, new TextSettingControl(Constants.Network_DeviceName)));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_DeviceType")/*"Device Type"*/, new ComboSettingsControl(Constants.Network_DeviceType, typeof(DeviceType), selectionDisplayName))); // replaced with ComboBox
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_DeviceModel")/*"Device Model"*/, new TextSettingControl(Constants.Network_DeviceModel)));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_Port")/*"Port"*/, new NumberSettingControl(Constants.Network_Port))); // replaced with NumberBox
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("Settings_Network_MulticastAddress")/*"Multicast Broadcast"*/, new TextSettingControl(Constants.Network_MulticastAddress)));
        }

        private void InitReceiveControls() {
            ReceiveSettingsStackPanel.Children.Add(new SettingsItemControl(LocalizeManager.GetLocalizedString("SettingsPage_Receive_SaveToFolder_Header")/*"Save directory"*/, new SaveLocationSettingControl()));
        }

        private void NavigateToDevPageButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(DevPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            InitControls();
        }

        #endregion Private Methods
    }
}