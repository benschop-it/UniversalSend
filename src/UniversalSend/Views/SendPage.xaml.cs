using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalSend.Controls.SendPage;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UniversalSend.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SendPage : Page
    {
        public SendPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SelectSendItemButtonsStackPanel.Children.Add(new SendItemButtonControl("\uEB9F","媒体"));
            SelectSendItemButtonsStackPanel.Children.Add(new SendItemButtonControl("\uEA37", "文本"));
            SelectSendItemButtonsStackPanel.Children.Add(new SendItemButtonControl("\uF0E3", "剪贴板"));
            SelectSendItemButtonsStackPanel.Children.Add(new SendItemButtonControl("\uE7C3", "文件"));
            SelectSendItemButtonsStackPanel.Children.Add(new SendItemButtonControl("\uE8B7", "文件夹"));
        }
    }
}
