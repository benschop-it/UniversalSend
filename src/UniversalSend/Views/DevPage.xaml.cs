using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Tasks;
using UniversalSend.Services;
using UniversalSend.Services.Interfaces;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Views {

    public sealed partial class DevPage : Page {

        private IStorageHelper _storageHelper;
        private ISendTaskManager _sendTaskManager;
        private IDeviceManager _deviceManager;
        private IServiceHttpServer _serviceHttpServer;

        #region Public Constructors

        public DevPage() {
            _storageHelper = App.Services.GetRequiredService<IStorageHelper>();
            _sendTaskManager = App.Services.GetRequiredService<ISendTaskManager>();
            _deviceManager = App.Services.GetRequiredService<IDeviceManager>();
            _serviceHttpServer = App.Services.GetRequiredService<IServiceHttpServer>();

            InitializeComponent();
        }

        public DevPage(
            IStorageHelper storageHelper, 
            ISendTaskManager sendTaskManager, 
            IDeviceManager deviceManager,
            IServiceHttpServer serviceHttpServer
        ) {
            _storageHelper = storageHelper ?? throw new ArgumentNullException(nameof(storageHelper));
            _sendTaskManager = sendTaskManager ?? throw new ArgumentNullException(nameof(sendTaskManager));
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
            _serviceHttpServer = serviceHttpServer ?? throw new ArgumentNullException(nameof(serviceHttpServer));

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void NavigateToReceivePageButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(FileReceivingPage));
        }

        private void OpenAppLocalFolderButton_Click(object sender, RoutedEventArgs e) {
            _storageHelper.LaunchAppLocalFolder();
        }

        private void OpenDownloadFolderButton_Click(object sender, RoutedEventArgs e) {
            // TODO: Implement opening of the system Downloads folder
        }

        private async Task SendSendRequestAsync() {
            List<IStorageFile> files = new List<IStorageFile>();
            files.Add(await ApplicationData.Current.LocalFolder.GetFileAsync("test.txt"));
            await _sendTaskManager.CreateSendTasksV1(files);

            // TODO: Replace with actual known device list
            IDevice device = _deviceManager.CreateDevice(
                "RM-1116_15169 (UWP)",
                "192.168.0.193",
                53317
            );

            await _sendTaskManager.SendSendRequestV1Async(device);
            await _sendTaskManager.SendSendTasksV1Async(device);
        }

        private async void SendSendRequestButton_Click(object sender, RoutedEventArgs e) {
            await SendSendRequestAsync();
        }

        private async Task StartHttpServerAsync() {
            await _serviceHttpServer.StartHttpServerAsync(53317);
        }

        private void StartHttpServerButton_Click(object sender, RoutedEventArgs e) {
            StartHttpServerAsync();
        }

        private async void WriteRequestContentToTestFileButton_Click(object sender, RoutedEventArgs e) {
        }

        #endregion Private Methods
    }
}