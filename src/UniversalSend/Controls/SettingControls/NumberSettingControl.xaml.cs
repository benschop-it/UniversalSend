using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalSend.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class NumberSettingControl : UserControl
    {
        public string SettingKey { get; set; }
        public NumberSettingControl(string key)
        {
            this.InitializeComponent();
            this.SettingKey = key;
        }

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainTextBox.BorderBrush = DefaultTextBox.BorderBrush;
            if (!string.IsNullOrEmpty(SettingKey) && MainTextBox.Text.All(char.IsDigit))
                Settings.SetSetting(SettingKey, Convert.ToInt32(MainTextBox.Text));
            else
                MainTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SettingKey))
                return;
            string settingValue = Settings.GetSettingContentAsString(SettingKey);

            if (string.IsNullOrEmpty(settingValue))
            {
                MainTextBox.Text = "不可用";
                MainTextBox.IsEnabled = false;
                return;
            }
            MainTextBox.Text = settingValue;

        }
    }
}
