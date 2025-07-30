using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using UniversalSend.Models;
using UniversalSend.Models.Interfaces;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// For more information on the "UserControl" item template, see https://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalSend.Controls.SettingControls {

    public sealed partial class NumberSettingControl : UserControl {
        private readonly ISettings _settings = App.Services.GetRequiredService<ISettings>();

        #region Public Constructors

        public NumberSettingControl(string key) {
            InitializeComponent();
            SettingKey = key;
        }

        #endregion Public Constructors

        #region Public Properties

        public string SettingKey { get; set; }

        #endregion Public Properties

        #region Private Methods

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            MainTextBox.BorderBrush = DefaultTextBox.BorderBrush;

            if (!string.IsNullOrEmpty(SettingKey) && MainTextBox.Text.All(char.IsDigit)) {
                _settings.SetSetting(SettingKey, Convert.ToInt32(MainTextBox.Text));
            } else {
                MainTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(SettingKey))
                return;

            string settingValue = _settings.GetSettingContentAsString(SettingKey);

            if (string.IsNullOrEmpty(settingValue)) {
                MainTextBox.Text = "Unavailable";
                MainTextBox.IsEnabled = false;
                return;
            }

            MainTextBox.Text = settingValue;
        }

        #endregion Private Methods
    }
}