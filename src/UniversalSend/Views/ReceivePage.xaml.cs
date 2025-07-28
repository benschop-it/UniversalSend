using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniversalSend.Models;
using UniversalSend.Models.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace UniversalSend.Views {

    public sealed partial class ReceivePage : Page {

        #region Public Constructors

        public ReceivePage() {
            InitializeComponent();
            RootGrid.Margin = UIManager.RootElementMargin;
        }

        #endregion Public Constructors

        #region Private Methods

        private void HistoryButton_Click(object sender, RoutedEventArgs e) {
            NavigateHelper.NavigateToHistoryPage();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            SetIcon();
            AliasTextBlock.Text = Settings.GetSettingContentAsString(Settings.Network_DeviceName);
            HashtagTextBlock.Text = "";

            List<string> IpAddrList = NetworkHelper.GetIPv4AddrList();
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