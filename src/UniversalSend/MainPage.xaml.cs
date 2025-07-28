using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
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
                List<SendTask> sendTasks = await ProcessShareActivatedAsync(e.Parameter as ShareOperation);
                SendTaskManager.SendTasks.AddRange(sendTasks);
                SendManager.SendCreatedEvent();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void Init() {
            Settings.InitUserSettings();
            Register.StartRegister();
            HistoryManager.InitHistoriesList();
            FavoriteManager.InitFavoritesData();
            UIManager.InitRootElementMargin();
            InitData();
        }

        private void InitData() {
            ProgramData.LocalDevice.Alias = Settings.GetSettingContentAsString(Settings.Network_DeviceName);
            ProgramData.LocalDevice.DeviceModel = Settings.GetSettingContentAsString(Settings.Network_DeviceModel);
            ProgramData.LocalDevice.DeviceType = Settings.GetSettingContentAsString(Settings.Network_DeviceType);
            ProgramData.LocalDevice.Port = (int)Settings.GetSettingContent(Settings.Network_Port);
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

        private async Task<List<SendTask>> ProcessShareActivatedAsync(ShareOperation shareOperation) {
            List<SendTask> sendTasks = new List<SendTask>();
            if (shareOperation.Data.Contains(StandardDataFormats.Text)) {
                string text = await shareOperation.Data.GetTextAsync();

                // To output the text from this example, you need a TextBlock control
                // with a name of "sharedContent".
                Debug.WriteLine($"ShareActivated-Text:{text}");
                sendTasks.Add(SendTaskManager.CreateSendTask(text));
            } else if (shareOperation.Data.Contains(StandardDataFormats.ApplicationLink)) {
                Uri uri = await shareOperation.Data.GetApplicationLinkAsync();
                Debug.WriteLine($"ShareActivated-ApplicationLink:{uri.ToString()}");
                sendTasks.Add(SendTaskManager.CreateSendTask(uri.ToString()));
            } else if (shareOperation.Data.Contains(StandardDataFormats.Bitmap)) {
                RandomAccessStreamReference accessStreamReference = await shareOperation.Data.GetBitmapAsync();
                Debug.WriteLine($"ShareActivated-Bitmap");
                var randomAccessStreamWithContentType = await (await shareOperation.Data.GetBitmapAsync()).OpenReadAsync();
                byte[] buffer = new byte[randomAccessStreamWithContentType.Size];
                await randomAccessStreamWithContentType.ReadAsync(buffer.AsBuffer(), (uint)randomAccessStreamWithContentType.Size, InputStreamOptions.None);
                StorageFile storageFile = await StorageHelper.CreateTempFile(Guid.NewGuid().ToString() + randomAccessStreamWithContentType.ContentType);
                await StorageHelper.WriteBytesToFileAsync(storageFile, buffer);
                sendTasks.Add(await SendTaskManager.CreateSendTask(storageFile));
            } else if (shareOperation.Data.Contains(StandardDataFormats.Html)) {
                string htmlStr = await shareOperation.Data.GetHtmlFormatAsync();
                Debug.WriteLine($"ShareActivated-Html:{htmlStr}");
                sendTasks.Add(SendTaskManager.CreateSendTask(htmlStr));
            } else if (shareOperation.Data.Contains(StandardDataFormats.Rtf)) {
                string rtfStr = await shareOperation.Data.GetRtfAsync();
                Debug.WriteLine($"ShareActivated-Rtf:{rtfStr}");
                sendTasks.Add(SendTaskManager.CreateSendTask(rtfStr));
            } else if (shareOperation.Data.Contains(StandardDataFormats.StorageItems)) {
                List<IStorageItem> items = (await shareOperation.Data.GetStorageItemsAsync()).ToList();
                Debug.WriteLine($"ShareActivated-StorageItems: number of items: {items.Count}");
                foreach (var item in items) {
                    if (item is StorageFile) {
                        sendTasks.Add(await SendTaskManager.CreateSendTask(item as StorageFile));
                    }
                }
            } else if (shareOperation.Data.Contains(StandardDataFormats.WebLink)) {
                Uri uri = await shareOperation.Data.GetWebLinkAsync();
                Debug.WriteLine($"ShareActivated-WebLink:{uri.ToString()}");
                sendTasks.Add(SendTaskManager.CreateSendTask(uri.ToString()));
            }
            return sendTasks;
        }

        #endregion Private Methods
    }
}