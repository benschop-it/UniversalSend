using System;
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
using static System.Net.WebRequestMethods;
using Microsoft.Toolkit.Uwp.Notifications;
using Restup.WebServer;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UniversalSend.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
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
    }
}
