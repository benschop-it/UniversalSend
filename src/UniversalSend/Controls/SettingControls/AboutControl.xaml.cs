using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace UniversalSend.Controls.SettingControls
{
    public sealed partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            this.InitializeComponent();

            About_AppMessageTextBlock.Text = string.Format("{0}.{1}.{2}.{3}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build,
                    Package.Current.Id.Version.Revision);
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
