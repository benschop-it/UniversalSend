using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalSend.Misc;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using UniversalSend.Strings;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace UniversalSend.Views {

    public sealed partial class HistoryPage : Page {

        private IHistoryManager _historyManager => App.Services.GetRequiredService<IHistoryManager>();
        private IStorageHelper _storageHelper => App.Services.GetRequiredService<IStorageHelper>();
        private ISettings _settings => App.Services.GetRequiredService<ISettings>();

        #region Public Constructors

        public HistoryPage() {
            this.InitializeComponent();
            UseInternalExplorer = Convert.ToBoolean(_settings.GetSettingContentAsString(Constants.Lab_UseInternalExplorer));
            initAsync();
        }

        #endregion Public Constructors

        #region Private Properties

        private IHistory RightTabedHistoryItem { get; set; }

        private bool UseInternalExplorer { get; set; }

        #endregion Private Properties

        #region Private Methods

        private void ClearHistoryButton_Click(object sender, RoutedEventArgs e) {
            _historyManager.HistoriesList.Clear();
            _historyManager.SaveHistoriesList();
            UpdateUI();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }

        private async Task initAsync() {
            try {
                StorageFolder storageFolder = await _storageHelper.GetReceiveStorageFolderAsync();
                if (storageFolder == null)
                    LaunchFolderButton.IsEnabled = false;
            } catch {
            }

            UpdateUI();
        }

        private async Task LaunchFileAsync(IHistory history) {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem(history.FutureAccessListToken)) {
                try {
                    StorageFile storageFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(history.FutureAccessListToken);
                    await Launcher.LaunchFileAsync(storageFile);
                } catch {
                    await MessageDialogManager.FileIsNotExistAsync();
                }
            }
        }

        private async void LaunchFolderButton_Click(object sender, RoutedEventArgs e) {
            if (UseInternalExplorer) {
                Frame.Navigate(typeof(ExplorerPage), null, new DrillInNavigationTransitionInfo());
            } else {
                var t = new FolderLauncherOptions();
                StorageFolder folder = await _storageHelper.GetReceiveStorageFolderAsync();
                await Launcher.LaunchFolderAsync(folder, t);
            }
        }

        private void ListViewMenuFlyout_Opened(object sender, object e) {
            if (RightTabedHistoryItem == null) {
                ListViewMenuFlyout.Hide();
                return;
            }
        }

        private async void MainListView_ItemClick(object sender, ItemClickEventArgs e) {
            IHistory history = e.ClickedItem as IHistory;
            if (history == null)
                return;
            if (String.IsNullOrEmpty(history.FutureAccessListToken)) {
                Frame.Navigate(typeof(ReceivedTextPage), history.File);
            } else {
                await LaunchFileAsync(history);
            }
        }

        private void MainListView_RightTapped(object sender, RightTappedRoutedEventArgs e) {
            RightTabedHistoryItem = (e.OriginalSource as FrameworkElement).DataContext as IHistory;
            if (String.IsNullOrEmpty(RightTabedHistoryItem.FutureAccessListToken)) {
                MenuFlyout_OpenFile.Visibility = Visibility.Collapsed;
                MenuFlyout_OpenFilePath.Visibility = Visibility.Collapsed;
            } else {
                MenuFlyout_OpenFile.Visibility = Visibility.Visible;
                MenuFlyout_OpenFilePath.Visibility = Visibility.Visible;
            }
        }

        private void MenuFlyout_Delete_Click(object sender, RoutedEventArgs e) {
            _historyManager.HistoriesList.Remove(RightTabedHistoryItem);
            _historyManager.SaveHistoriesList();
            UpdateUI();
        }

        private void MenuFlyout_Info_Click(object sender, RoutedEventArgs e) {
            /*To-Do: History details popup*/
        }

        private async void MenuFlyout_OpenFile_Click(object sender, RoutedEventArgs e) {
            await LaunchFileAsync(RightTabedHistoryItem);
        }

        private async void MenuFlyout_OpenFilePath_Click(object sender, RoutedEventArgs e) {
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem(RightTabedHistoryItem.FutureAccessListToken)) {
                try {
                    StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(RightTabedHistoryItem.FutureAccessListToken);
                    await Launcher.LaunchFolderPathAsync(file.Path.Substring(0, file.Path.LastIndexOf("\\")));
                } catch {
                    await MessageDialogManager.FileIsNotExistAsync();
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            if (UseInternalExplorer) {
                MenuFlyout_OpenFilePath.IsEnabled = false;
            }
        }

        private void UpdateUI() {
            MainListView.ItemsSource = null;
            List<IHistory> list = _historyManager.HistoriesList.ToList();
            list.Reverse();
            MainListView.ItemsSource = list;
        }

        #endregion Private Methods
    }
}