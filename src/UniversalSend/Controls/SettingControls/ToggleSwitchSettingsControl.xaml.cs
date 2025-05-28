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

namespace UniversalSend.Controls.SettingControls
{
    public sealed partial class ToggleSwitchSettingsControl : UserControl
    {
        string Key { get; set; }

        public ToggleSwitchSettingsControl(string key)
        {
            this.InitializeComponent();
            Key = key;
        }

        private void MainToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Settings.SetSetting(Key, MainToggleSwitch.IsOn);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Key))
                return;
            string settingValue = Settings.GetSettingContentAsString(Key);

            if (string.IsNullOrEmpty(settingValue))
            {
                MainToggleSwitch.IsEnabled = false;
                return;
            }
            MainToggleSwitch.IsOn = Convert.ToBoolean(settingValue);
        }
    }
}
