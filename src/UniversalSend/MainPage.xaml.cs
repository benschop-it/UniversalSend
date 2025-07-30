using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Interfaces;
using UniversalSend.Models;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using UniversalSend.Strings;
using UniversalSend.Views;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 describes the "Blank Page" item template

namespace UniversalSend {

    /// <summary>
    /// A blank page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {

        #region Private Fields

        private bool _normalLaunch = true;
        private ISendTaskManager _sendTaskManager => App.Services.GetRequiredService<ISendTaskManager>();
        private ISendManager _sendManager => App.Services.GetRequiredService<ISendManager>();
        private IHistoryManager _historyManager => App.Services.GetRequiredService<IHistoryManager>();
        private IFavoriteManager _favoriteManager => App.Services.GetRequiredService<IFavoriteManager>();
        private IRegister _register => App.Services.GetRequiredService<IRegister>();
        private IStorageHelper _storageHelper => App.Services.GetRequiredService<IStorageHelper>();
        private ISettings _settings => App.Services.GetRequiredService<ISettings>();
        private ISystemHelper _systemHelper => App.Services.GetRequiredService<ISystemHelper>();
        private IUIManager _uiManager => App.Services.GetRequiredService<IUIManager>();
        private IDeviceManager _deviceManager => App.Services.GetRequiredService<IDeviceManager>();

        #endregion Private Fields

        #region Public Constructors

        public MainPage() {
            this.InitializeComponent();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            if (e.Parameter == null) {
                return;
            }

            if (e.Parameter is ShareOperation) {
                _normalLaunch = false;
                List<ISendTask> sendTasks = await ProcessShareActivatedAsync(e.Parameter as ShareOperation);
                _sendTaskManager.SendTasks.AddRange(sendTasks);
                _sendManager.SendCreatedEvent();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void Init() {
            _settings.InitUserSettings();
            _register.StartRegister();
            _historyManager.InitHistoriesList();
            _favoriteManager.InitFavoritesData();
            _uiManager.InitRootElementMargin();
            InitData();
        }

        private void InitData() {
            IDevice localDevice = _deviceManager.GetLocalDevice();

            localDevice.Alias = _settings.GetSettingContentAsString(Constants.Network_DeviceName);
            localDevice.DeviceModel = _settings.GetSettingContentAsString(Constants.Network_DeviceModel);
            localDevice.DeviceType = _settings.GetSettingContentAsString(Constants.Network_DeviceType);
            localDevice.Port = (int)_settings.GetSettingContent(Constants.Network_Port);
        }

        private void NavigateToRootPage() {
            string deviceFamilyVersion = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong build = (version & 0x00000000FFFF0000L) >> 16;
            Debug.WriteLine($"System version build number: {build}");
            if (build >= 16299) {
                Frame.Navigate(typeof(RootPage));
            } else {
                Frame.Navigate(typeof(RootPage15063));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            Init();
            if (_normalLaunch) {
                NavigateToRootPage();
            } else {
                Frame.Navigate(typeof(SendPage), "ShareActivated");
            }
        }

        private async Task<List<ISendTask>> ProcessShareActivatedAsync(ShareOperation shareOperation) {
            List<ISendTask> sendTasks = new List<ISendTask>();
            if (shareOperation.Data.Contains(StandardDataFormats.Text)) {
                string text = await shareOperation.Data.GetTextAsync();

                // To output the text from this example, you need a TextBlock control
                // with a name of "sharedContent".
                Debug.WriteLine($"ShareActivated-Text:{text}");
                sendTasks.Add(_sendTaskManager.CreateSendTask(text));
            } else if (shareOperation.Data.Contains(StandardDataFormats.ApplicationLink)) {
                Uri uri = await shareOperation.Data.GetApplicationLinkAsync();
                Debug.WriteLine($"ShareActivated-ApplicationLink:{uri.ToString()}");
                sendTasks.Add(_sendTaskManager.CreateSendTask(uri.ToString()));
            } else if (shareOperation.Data.Contains(StandardDataFormats.Bitmap)) {
                RandomAccessStreamReference accessStreamReference = await shareOperation.Data.GetBitmapAsync();
                Debug.WriteLine($"ShareActivated-Bitmap");
                var randomAccessStreamWithContentType = await (await shareOperation.Data.GetBitmapAsync()).OpenReadAsync();
                byte[] buffer = new byte[randomAccessStreamWithContentType.Size];
                await randomAccessStreamWithContentType.ReadAsync(buffer.AsBuffer(), (uint)randomAccessStreamWithContentType.Size, InputStreamOptions.None);
                StorageFile storageFile = await _storageHelper.CreateTempFile(Guid.NewGuid().ToString() + randomAccessStreamWithContentType.ContentType);
                await _storageHelper.WriteBytesToFileAsync(storageFile, buffer);
                sendTasks.Add(await _sendTaskManager.CreateSendTask(storageFile));
            } else if (shareOperation.Data.Contains(StandardDataFormats.Html)) {
                string htmlStr = await shareOperation.Data.GetHtmlFormatAsync();
                Debug.WriteLine($"ShareActivated-Html:{htmlStr}");
                sendTasks.Add(_sendTaskManager.CreateSendTask(htmlStr));
            } else if (shareOperation.Data.Contains(StandardDataFormats.Rtf)) {
                string rtfStr = await shareOperation.Data.GetRtfAsync();
                Debug.WriteLine($"ShareActivated-Rtf:{rtfStr}");
                sendTasks.Add(_sendTaskManager.CreateSendTask(rtfStr));
            } else if (shareOperation.Data.Contains(StandardDataFormats.StorageItems)) {
                List<IStorageItem> items = (await shareOperation.Data.GetStorageItemsAsync()).ToList();
                Debug.WriteLine($"ShareActivated-StorageItems: number of items: {items.Count}");
                foreach (var item in items) {
                    if (item is StorageFile) {
                        sendTasks.Add(await _sendTaskManager.CreateSendTask(item as StorageFile));
                    }
                }
            } else if (shareOperation.Data.Contains(StandardDataFormats.WebLink)) {
                Uri uri = await shareOperation.Data.GetWebLinkAsync();
                Debug.WriteLine($"ShareActivated-WebLink:{uri.ToString()}");
                sendTasks.Add(_sendTaskManager.CreateSendTask(uri.ToString()));
            }
            return sendTasks;
        }

        #endregion Private Methods
    }
}