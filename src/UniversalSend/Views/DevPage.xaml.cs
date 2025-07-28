using Restup.WebServer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Tasks;
using UniversalSend.Services;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Views {

    public sealed partial class DevPage : Page {

        #region Public Constructors

        public DevPage() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void NavigateToReceivePageButton_Click(object sender, RoutedEventArgs e) {
            Frame.Navigate(typeof(FileReceivingPage));
        }

        private void OpenAppLocalFolderButton_Click(object sender, RoutedEventArgs e) {
            StorageHelper.LaunchAppLocalFolder();
        }

        private void OpenDownloadFolderButton_Click(object sender, RoutedEventArgs e) {
            // TODO: Implement opening of the system Downloads folder
        }

        private async Task SendSendRequestAsync() {
            List<StorageFile> files = new List<StorageFile>();
            files.Add(await ApplicationData.Current.LocalFolder.GetFileAsync("test.txt"));
            await SendTaskManager.CreateSendTasks(files);

            // TODO: Replace with actual known device list
            Device device = new Device {
                Alias = "RM-1116_15169 (UWP)",
                IP = "192.168.0.193",
                Port = 53317,
            };

            await SendTaskManager.SendSendRequestAsync(device);
            await SendTaskManager.SendSendTasksAsync(device);
        }

        private async void SendSendRequestButton_Click(object sender, RoutedEventArgs e) {
            await SendSendRequestAsync();
        }

        private async Task StartHttpServerAsync() {
            ServiceHttpServer server = new ServiceHttpServer();
            await server.StartHttpServerAsync(53317);
        }

        private void StartHttpServerButton_Click(object sender, RoutedEventArgs e) {
            StartHttpServerAsync();
        }
        private async void WriteRequestContentToTestFileButton_Click(object sender, RoutedEventArgs e) {
            await StorageHelper.WriteTestFileAsync(RestupTest.requestContent);
        }

        #endregion Private Methods
    }
}