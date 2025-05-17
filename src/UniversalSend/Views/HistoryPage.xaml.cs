using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.System;
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
    public sealed partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            this.InitializeComponent();
            initAsync();
        }

        async Task initAsync()
        {
            try
            {
                StorageFolder storageFolder = await StorageHelper.GetReceiveStoageFolderAsync();
                if (storageFolder == null)
                    LaunchFolderButton.IsEnabled = false;
            }
            catch
            {

            }

            UpdateUI();

        }

        void UpdateUI()
        {
            MainListView.ItemsSource = null;
            List<History> list = HistoryManager.HistoriesList.ToList();
            list.Reverse();
            MainListView.ItemsSource = list;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void LaunchFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var t = new FolderLauncherOptions();
            StorageFolder folder = await StorageHelper.GetReceiveStoageFolderAsync();
            await Launcher.LaunchFolderAsync(folder, t);
        }

        private void ClearHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryManager.HistoriesList.Clear();
            HistoryManager.SaveHistoriesList();
            UpdateUI();
        }

        private async void MainListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            History history = e.ClickedItem as History;
            if (history == null)
                return;
            if(String.IsNullOrEmpty(history.FutureAccessListToken))
            {
                Frame.Navigate(typeof(ReceivedTextPage),history.File);
            }
            else
            {
                await LaunchFileAsync(history);
            }
        }

        async Task LaunchFileAsync(History history)
        {
            if(StorageApplicationPermissions.FutureAccessList.ContainsItem(history.FutureAccessListToken))
            {
                try
                {
                    StorageFile storageFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(history.FutureAccessListToken);
                    await Launcher.LaunchFileAsync(storageFile);
                }
                catch
                {
                    await MessageDialogManager.FileIsNotExistAsync();
                }
            }
                
        }

        private void MainListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            RightTabedHistoryItem = (e.OriginalSource as FrameworkElement).DataContext as History;
            
            if(String.IsNullOrEmpty(RightTabedHistoryItem.FutureAccessListToken))
            {
                MenuFlyout_OpenFile.Visibility = Visibility.Collapsed;
                MenuFlyout_OpenFilePath.Visibility = Visibility.Collapsed;
            }else
            {
                MenuFlyout_OpenFile.Visibility = Visibility.Visible;
                MenuFlyout_OpenFilePath.Visibility = Visibility.Visible;
            }
        }

        History RightTabedHistoryItem { get; set; }

        private void MenuFlyout_Delete_Click(object sender, RoutedEventArgs e)
        {
            HistoryManager.HistoriesList.Remove(RightTabedHistoryItem);
            HistoryManager.SaveHistoriesList();
            UpdateUI();

        }

        private void MenuFlyout_Info_Click(object sender, RoutedEventArgs e)
        {
            /*To-Do:历史记录详细信息弹窗*/
        }

        private async void MenuFlyout_OpenFilePath_Click(object sender, RoutedEventArgs e)
        {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem(RightTabedHistoryItem.FutureAccessListToken))
            {
                try
                {
                    StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(RightTabedHistoryItem.FutureAccessListToken);
                    await Launcher.LaunchFolderPathAsync(file.Path.Substring(0, file.Path.LastIndexOf("\\")));
                }
                catch
                {
                    await MessageDialogManager.FileIsNotExistAsync();
                }
            }
        }

        private async void MenuFlyout_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            await LaunchFileAsync(RightTabedHistoryItem);
        }

        private void ListViewMenuFlyout_Opened(object sender, object e)
        {
            if (RightTabedHistoryItem == null)
            {
                ListViewMenuFlyout.Hide();
                return;
            }
        }
    }
}
