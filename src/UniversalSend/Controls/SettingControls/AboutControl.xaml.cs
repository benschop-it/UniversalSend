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

// For more information on the "UserControl" item template, see https://go.microsoft.com/fwlink/?LinkId=234236

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
                About_UpdateMessage.Text = "Failed to check for updates. Please check your network and try again!";
            }

            if (Convert.ToInt32(appVersion) < Convert.ToInt32(result.Substring(result.LastIndexOf(".") + 1)))
            {
                About_UpdateMessage.Text = "An update is available. Please visit the project repository to download the latest version.";
            }
            else
            {
                About_UpdateMessage.Text = "You are using the latest version.";
            }

            CheckForUpdateProgressBar.Visibility = Visibility.Collapsed;
        }
    }
}
