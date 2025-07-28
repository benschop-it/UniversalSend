using UniversalSend.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.SettingControls {

    public sealed partial class TextSettingControl : UserControl {

        #region Public Constructors

        public TextSettingControl(string key) {
            InitializeComponent();
            SettingKey = key;
        }

        #endregion Public Constructors

        #region Public Properties

        public string SettingKey { get; set; }

        #endregion Public Properties

        #region Private Methods

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (!string.IsNullOrEmpty(SettingKey)) {
                Settings.SetSetting(SettingKey, MainTextBox.Text);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(SettingKey)) {
                return;
            }

            string settingValue = Settings.GetSettingContentAsString(SettingKey);

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