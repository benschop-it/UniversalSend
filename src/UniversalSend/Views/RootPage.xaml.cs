using System;
using System.Threading.Tasks;
using UniversalSend.Controls.ContentDialogControls;
using UniversalSend.Models;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Managers;
using UniversalSend.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace UniversalSend.Views {

    public sealed partial class RootPage : Page {

        #region Private Fields

        private bool _isInited = false;

        #endregion Private Fields

        #region Public Constructors

        public RootPage() {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        #endregion Public Constructors

        #region Public Properties

        public static ServiceHttpServer ServiceServer { get; set; }

        #endregion Public Properties

        #region Private Methods

        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args) {
            switch (((NavigationViewItem)sender.SelectedItem).Tag.ToString()) {
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

        private async void NavigateHelper_NavigateToHistoryPageEvent(object sender, EventArgs e) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Frame.Navigate(typeof(HistoryPage), null, new DrillInNavigationTransitionInfo());
            });
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            if (!_isInited) {
                _isInited = true;
                ((NavigationViewItem)MainNavigationView.SettingsItem).Tag = "Settings";
                MainNavigationView.SelectedItem = ReceivePageItem;
                await StartHttpServerAsync();
                ReceiveManager.SendRequestReceived += ReceiveManager_SendRequestReceived;
                SendManager.SendPrepared += SendManager_SendPrepared;
                NavigateHelper.NavigateToHistoryPageEvent += NavigateHelper_NavigateToHistoryPageEvent;
            }
            while (await StorageHelper.GetReceiveStorageFolderAsync() == null) {
                await ProgramData.ContentDialogManager.ShowContentDialogAsync(new PickReceiveFolderControl());
            }
        }

        private async void ReceiveManager_SendRequestReceived(object sender, EventArgs e) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                if (/*ReceiveManager.QuickSave == ReceiveManager.QuickSaveMode.Off*/false) {
                    Frame.Navigate(typeof(ConfirmReceiptPage));
                } else {
                    Frame.Navigate(typeof(FileReceivingPage));
                }
            });
        }

        private async void SendManager_SendPrepared(object sender, EventArgs e) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Frame.Navigate(typeof(FileSendingPage), sender);
            });
        }

        private async Task StartHttpServerAsync() {
            ProgramData.ServiceServer = new ServiceHttpServer();
            var port = (int)Settings.GetSettingContent(Settings.Network_Port);
            await ((ServiceHttpServer)ProgramData.ServiceServer).StartHttpServerAsync(port);
        }

        #endregion Private Methods
    }
}