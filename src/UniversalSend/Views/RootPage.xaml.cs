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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UniversalSend.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RootPage : Page
    {
        public static ServiceHttpServer ServiceServer { get; set; }

        bool isInited = false;
        
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
            if(!isInited)
            {
                isInited = true;
                ((NavigationViewItem)MainNavigationView.SettingsItem).Tag = "Settings";
                MainNavigationView.SelectedItem = ReceivePageItem;
                StartHttpServerAsync();
                ReceiveManager.SendRequestReceived += ReceiveManager_SendRequestReceived;
                SendManager.SendPrepared += SendManager_SendPrepared;
                NavigateHelper.NavigateToHistoryPageEvent += NavigateHelper_NavigateToHistoryPageEvent;
            }
        }

        private async void NavigateHelper_NavigateToHistoryPageEvent(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Frame.Navigate(typeof(HistoryPage));
            });
        }

        private async void SendManager_SendPrepared(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Frame.Navigate(typeof(FileSendingPage), sender);
            });
        }

        private async void ReceiveManager_SendRequestReceived(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Frame.Navigate(typeof(FileReceivingPage));
            }); 
        }

        async Task StartHttpServerAsync()
        {
            ProgramData.ServiceServer = new ServiceHttpServer();
            await ((ServiceHttpServer)ProgramData.ServiceServer).StartHttpServerAsync((int)Settings.GetSettingContent(Settings.Network_Port));
        }
    }
}
