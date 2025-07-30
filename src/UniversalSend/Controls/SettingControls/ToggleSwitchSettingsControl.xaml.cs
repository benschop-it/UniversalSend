using Microsoft.Extensions.DependencyInjection;
using System;
using UniversalSend.Models;
using UniversalSend.Models.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.SettingControls {

    public sealed partial class ToggleSwitchSettingsControl : UserControl {
        private readonly ISettings _settings = App.Services.GetRequiredService<ISettings>();


        #region Public Constructors

        public ToggleSwitchSettingsControl(string key) {
            InitializeComponent();
            Key = key;
        }

        #endregion Public Constructors

        #region Private Properties

        private string Key { get; set; }

        #endregion Private Properties

        #region Private Methods

        private void MainToggleSwitch_Toggled(object sender, RoutedEventArgs e) {
            _settings.SetSetting(Key, MainToggleSwitch.IsOn);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(Key)) {
                return;
            }

            string settingValue = _settings.GetSettingContentAsString(Key);

            if (string.IsNullOrEmpty(settingValue)) {
                MainToggleSwitch.IsEnabled = false;
                return;
            }
            MainToggleSwitch.IsOn = Convert.ToBoolean(settingValue);
        }

        #endregion Private Methods
    }
}