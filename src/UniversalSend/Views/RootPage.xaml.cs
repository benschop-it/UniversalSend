using System;
using System.Threading.Tasks;
using UniversalSend.Controls.ContentDialogControls;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using UniversalSend.Services;
using Microsoft.Extensions.DependencyInjection;
using UniversalSend.Strings;
using UniversalSend.Misc;
using UniversalSend.Services.Interfaces;

namespace UniversalSend.Views {

    public sealed partial class RootPage : Page {

        #region Private Fields

        private bool _isInited = false;
        private IReceiveManager _receiveManager => App.Services.GetRequiredService<IReceiveManager>();
        private ISendManager _sendManager => App.Services.GetRequiredService<ISendManager>();
        private IStorageHelper _storageHelper => App.Services.GetRequiredService<IStorageHelper>();
        private IServiceHttpServer _serviceHttpServer => App.Services.GetRequiredService<IServiceHttpServer>();
        private IContentDialogManager _contentDialogManager => App.Services.GetRequiredService<IContentDialogManager>();
        private ISettings _settings => App.Services.GetRequiredService<ISettings>();

        #endregion Private Fields

        #region Public Constructors

        public RootPage() {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        #endregion Public Constructors

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
                _receiveManager.SendRequestReceived += ReceiveManager_SendRequestReceived;
                _sendManager.SendPrepared += SendManager_SendPrepared;
                NavigateHelper.NavigateToHistoryPageEvent += NavigateHelper_NavigateToHistoryPageEvent;
            }
            while (await _storageHelper.GetReceiveStorageFolderAsync() == null) {
                await _contentDialogManager.ShowContentDialogAsync(new PickReceiveFolderControl());
            }
        }

        private async void ReceiveManager_SendRequestReceived(object sender, EventArgs e) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                if (/*ReceiveManager.QuickSave == ReceiveManager.QuickSaveMode.Off*/true) {
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
            //ProgramData.ServiceServer = new ServiceHttpServer();
            var port = (int)_settings.GetSettingContent(Constants.Network_Port);
            await _serviceHttpServer.StartHttpServerAsync(port);
        }

        #endregion Private Methods
    }
}