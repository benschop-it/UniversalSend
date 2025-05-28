using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UniversalSend.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ReceivePage : Page
    {
        public ReceivePage()
        {
            this.InitializeComponent();
            RootGrid.Margin = UIManager.RootElementMargin;
            
            
        }

        void SetIcon()
        {
            if (App.Current.RequestedTheme == ApplicationTheme.Light)
            {
                //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync();
                IconImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/UniversalSendNew_Dark.png"));
            }
            else
            {
                IconImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/UniversalSendNew.png"));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetIcon();
            AliasTextBlock.Text = Settings.GetSettingContentAsString(Settings.Network_DeviceName);
            HashtagTextBlock.Text = "";

            List<string>IpAddrList = NetworkHelper.GetIPv4AddrList();
            foreach(string ip in IpAddrList)
            {
                //Debug.WriteLine(ip);
                HashtagTextBlock.Text += $"#{ip.Substring(ip.LastIndexOf(".")+1)} ";
            }
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateHelper.NavigateToHitoryPage();
        }
    }
}
