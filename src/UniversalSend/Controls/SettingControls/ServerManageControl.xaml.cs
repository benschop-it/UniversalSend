using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Services;
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
    public sealed partial class ServerManageControl : UserControl
    {
        public ServerManageControl()
        {
            this.InitializeComponent();
        }

        private async void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            ((ServiceHttpServer)ProgramData.ServiceServer).StopHttpServer();
            if(await ((ServiceHttpServer)ProgramData.ServiceServer).StartHttpServerAsync(Convert.ToInt32(Settings.GetSettingContentAsString(Settings.Network_Port))))
            {
                StopButton.IsEnabled = true;
                RestartButtonIcon.Glyph = "\uE72C";
            }
            
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            ((ServiceHttpServer)ProgramData.ServiceServer).StopHttpServer();
            StopButton.IsEnabled = false;
            RestartButtonIcon.Glyph = "\uE768";
        }
    }
}
