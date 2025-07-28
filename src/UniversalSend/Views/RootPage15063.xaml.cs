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

    public sealed partial class RootPage15063 : Page {

        #region Private Fields

        private bool _isInited = false;

        #endregion Private Fields

        #region Public Constructors

        public RootPage15063() {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        #endregion Public Constructors

        #region Private Methods

        private void BottomAppBarReceiveButton_Click(object sender, RoutedEventArgs e) {
            MainFrame.Navigate(typeof(ReceivePage));
        }

        private void BottomAppBarSendButton_Click(object sender, RoutedEventArgs e) {
            MainFrame.Navigate(typeof(SendPage));
        }

        private void BottomAppBarSettingsButton_Click(object sender, RoutedEventArgs e) {
            MainFrame.Navigate(typeof(SettingsPage));
        }

        private async void NavigateHelper_NavigateToHistoryPageEvent(object sender, EventArgs e) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Frame.Navigate(typeof(HistoryPage), null, new DrillInNavigationTransitionInfo());
            });
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            MainFrame.Navigate(typeof(ReceivePage));
            if (!_isInited) {
                _isInited = true;
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
                Frame.Navigate(typeof(FileReceivingPage), sender);
            });
        }

        private async void SendManager_SendPrepared(object sender, EventArgs e) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Frame.Navigate(typeof(FileSendingPage), sender);
            });
        }

        private async Task StartHttpServerAsync() {
            ProgramData.ServiceServer = new ServiceHttpServer();
            await ((ServiceHttpServer)ProgramData.ServiceServer).StartHttpServerAsync((int)Settings.GetSettingContent(Settings.Network_Port));
        }

        #endregion Private Methods
    }
}