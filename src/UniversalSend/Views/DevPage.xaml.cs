﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Microsoft.Toolkit.Uwp.Notifications;
using Restup.WebServer;
using UniversalSend.Services;
using UniversalSend.Models.Tasks;
using UniversalSend.Models.Data;

// https://go.microsoft.com/fwlink/?LinkId=234238 describes the Blank Page item template

namespace UniversalSend.Views
{
    /// <summary>
    /// A blank page that can be used independently or navigated to within a Frame.
    /// </summary>
    public sealed partial class DevPage : Page
    {
        public DevPage()
        {
            this.InitializeComponent();
        }

        private void StartHttpServerButton_Click(object sender, RoutedEventArgs e)
        {
            StartHttpServerAsync();
        }

        async Task StartHttpServerAsync()
        {
            ServiceHttpServer server = new ServiceHttpServer();
            await server.StartHttpServerAsync(53317);
        }

        private void OpenAppLocalFolderButton_Click(object sender, RoutedEventArgs e)
        {
            StorageHelper.LaunchAppLocalFolder();
        }

        private async void WriteRequestContentToTestFileButton_Click(object sender, RoutedEventArgs e)
        {
            await StorageHelper.WriteTestFileAsync(RestupTest.requestContent);
        }

        private void NavigateToReceivePageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FileReceivingPage));
        }

        private void OpenDownloadFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement opening of the system Downloads folder
        }

        private async void SendSendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            await SendSendRequestAsync();
        }

        async Task SendSendRequestAsync()
        {
            List<StorageFile> files = new List<StorageFile>();
            files.Add(await ApplicationData.Current.LocalFolder.GetFileAsync("test.txt"));
            await SendTaskManager.CreateSendTasks(files);

            // TODO: Replace with actual known device list
            Device device = new Device
            {
                Alias = "Mi12s",
                IP = "192.168.55.150",
                Port = 53317,
            };

            await SendTaskManager.SendSendRequestAsync(device);
            await SendTaskManager.SendSendTasksAsync(device);
        }
    }
}
