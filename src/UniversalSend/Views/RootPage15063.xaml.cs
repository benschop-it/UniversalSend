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
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace UniversalSend.Views {

    public sealed partial class RootPage15063 : Page {

        private IReceiveManager _receiveManager => App.Services.GetRequiredService<IReceiveManager>();
        private ISendManager _sendManager => App.Services.GetRequiredService<ISendManager>();
        private IStorageHelper _storageHelper => App.Services.GetRequiredService<IStorageHelper>();
        private IServiceHttpServer _serviceHttpServer => App.Services.GetRequiredService<IServiceHttpServer>();
        private IContentDialogManager _contentDialogManager => App.Services.GetRequiredService<IContentDialogManager>();
        private ISettings _settings => App.Services.GetRequiredService<ISettings>();

        #region Private Fields

        private bool _isInited = false;

        #endregion Private Fields

        public int ProgressPercent { get; private set; }
        public string ProgressText { get; private set; }

        #region Public Constructors

        public RootPage15063() {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            _serviceHttpServer.HttpRequestProgressChanged += OnServiceHttpServerHttpRequestProgressChanged;
        }

        private async void OnServiceHttpServerHttpRequestProgressChanged(object sender, HttpParseProgressEventArgs e) {
            // Marshal to UI thread for safety
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                if (e.ContentLength == null || e.ContentLength.Value == 0) {
                    ProgressText = $"Receiving…";
                } else {
                    if (e.Percent.HasValue) {
                        ProgressPercent = e.Percent.Value;
                        ProgressText = $"Receiving {e.Percent.Value}% ({e.Received}/{e.ContentLength} bytes)";
                    } else {
                        ProgressText = $"Receiving… {e.Received} bytes";
                    }
                }

                Debug.WriteLine($"{ProgressText}");
            });
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
                Frame.Navigate(typeof(FileReceivingPage), sender);
            });
        }

        private async void SendManager_SendPrepared(object sender, EventArgs e) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Frame.Navigate(typeof(FileSendingPage), sender);
            });
        }

        private async Task StartHttpServerAsync() {
            var port = (int)_settings.GetSettingContent(Constants.Network_Port);
            await _serviceHttpServer.StartHttpServerAsync(port);
        }

        #endregion Private Methods
    }
}