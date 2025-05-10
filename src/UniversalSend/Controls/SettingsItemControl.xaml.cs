using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace UniversalSend.Controls
{
    public sealed partial class SettingsItemControl : UserControl
    {
        public FrameworkElement SettingControl { get; set; }

        public string Header { get; set; }
        public SettingsItemControl(string header,FrameworkElement settingControl)
        {
            this.InitializeComponent();
            this.Header = header;
            this.SettingControl = settingControl;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SettingControlGrid.Children.Clear();
            SettingControlGrid.Children.Add(SettingControl);
        }
    }
}
