using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UniversalSend.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RootPage : Page
    {
        public RootPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            
        }

        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            switch(((NavigationViewItem)sender.SelectedItem).Tag.ToString())
            {
                case "Receive":
                    MainFrame.Navigate(typeof(ReceivePage));
                    break;
                case "Send":
                    MainFrame.Navigate(typeof(SendPage));
                    break;
                case "Settings":
                    MainFrame.Navigate(typeof(SettingsPage));
                    break;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((NavigationViewItem)MainNavigationView.SettingsItem).Tag = "Settings";
            MainNavigationView.SelectedItem = ReceivePageItem;
            StartHttpServerAsync();
        }

        async Task StartHttpServerAsync()
        {
            ProgramData.ServiceServer = new ServiceHttpServer();
            await ProgramData.ServiceServer.StartHttpServerAsync(53317);
        }
    }
}
