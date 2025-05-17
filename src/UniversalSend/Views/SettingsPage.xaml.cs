using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Controls;
using UniversalSend.Controls.SettingControls;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using Windows.ApplicationModel;
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
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        void InitControls()
        {
            InitNetworkControls();
            InitReceiveControls();
            InitAboutControls();
        }

        void InitReceiveControls()
        {
            ReceiveSettingsStackPanel.Children.Add(new SettingsItemControl("保存目录", new SaveLocationSettingControl()));
        }

        void InitNetworkControls()
        {
            Dictionary<int, string> selectionDisplayName = new Dictionary<int, string>();
            selectionDisplayName.Add((int)DeviceManager.DeviceType.mobile, "手机/平板");
            selectionDisplayName.Add((int)DeviceManager.DeviceType.desktop, "电脑");
            selectionDisplayName.Add((int)DeviceManager.DeviceType.web, "网页");
            selectionDisplayName.Add((int)DeviceManager.DeviceType.headless, "终端");
            selectionDisplayName.Add((int)DeviceManager.DeviceType.server, "服务器");

            

            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("服务器",new ServerManageControl()));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("别名",new TextSettingControl(Settings.Network_DeviceName)));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("设备类型",new ComboSettingsControl(Settings.Network_DeviceType,typeof(DeviceManager.DeviceType),selectionDisplayName)));//替换为下拉框
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("设备型号",new TextSettingControl(Settings.Network_DeviceModel)));
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("端口",new NumberSettingControl(Settings.Network_Port)));//替换为NumberBox
            NetworkSettingsStackPanel.Children.Add(new SettingsItemControl("多线程广播",new TextSettingControl(Settings.Network_MulticaastAddress)));
        }

        void InitAboutControls()
        {
            About_AppMessageTextBlock.Text = string.Format("{0}.{1}.{2}.{3}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build,
                    Package.Current.Id.Version.Revision);

        }

        private void NavigateToDevPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DevPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitControls();
        }

        private async void About_CheckForUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            await CheckForUpdateAsync();
        }

        async Task CheckForUpdateAsync()
        {
            CheckForUpdateProgressBar.Visibility = Visibility.Visible;
            var http = new HttpClient();
            var response = await http.GetAsync("https://pigeon-ming.github.io/Versions/universalsend.txt");
            var result = await response.Content.ReadAsStringAsync();
            string appVersion = Package.Current.Id.Version.Build.ToString();
            if (String.IsNullOrEmpty(result) || response.IsSuccessStatusCode == false)
            {
                About_UpdateMessage.Text = "检查更新失败，请检查网络后再试！";
            }
            //Debug.WriteLine(result.Substring(result.LastIndexOf(".") + 1, result.Length - result.LastIndexOf(".") - 1));
            if (Convert.ToInt32(appVersion) < Convert.ToInt32(result.Substring(result.LastIndexOf(".") + 1, result.Length - result.LastIndexOf(".") - 1)))
            {

                About_UpdateMessage.Text = "有可用更新，请前往项目仓库下载最新版本";
            }
            else
            {
                About_UpdateMessage.Text = "您使用的是最新版本";

            }
            CheckForUpdateProgressBar.Visibility = Visibility.Collapsed;
        }
    }
}
