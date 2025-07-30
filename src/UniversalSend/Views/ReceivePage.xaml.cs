using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniversalSend.Interfaces;
using UniversalSend.Misc;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace UniversalSend.Views {

    public sealed partial class ReceivePage : Page {

        private ISettings _settings => App.Services.GetRequiredService<ISettings>();
        private INetworkHelper _networkHelper => App.Services.GetRequiredService<INetworkHelper>();
        private IUIManager _uiManager => App.Services.GetRequiredService<IUIManager>();

        #region Public Constructors

        public ReceivePage() {
            InitializeComponent();
            RootGrid.Margin = _uiManager.RootElementMargin;
        }

        #endregion Public Constructors

        #region Private Methods

        private void HistoryButton_Click(object sender, RoutedEventArgs e) {
            NavigateHelper.NavigateToHistoryPage();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            SetIcon();
            AliasTextBlock.Text = _settings.GetSettingContentAsString(Constants.Network_DeviceName);
            HashtagTextBlock.Text = "";

            List<string> IpAddrList = _networkHelper.GetIPv4AddrList();
            foreach (string ip in IpAddrList) {
                Debug.WriteLine("IPv4 address: " + ip);
                HashtagTextBlock.Text += $"#{ip.Substring(ip.LastIndexOf(".") + 1)} ";
            }
        }

        private void SetIcon() {
            if (App.Current.RequestedTheme == ApplicationTheme.Light) {
                IconImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/UniversalSendNew_Dark.png"));
            } else {
                IconImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/UniversalSendNew.png"));
            }
        }

        #endregion Private Methods
    }
}