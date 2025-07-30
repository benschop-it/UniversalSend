using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.SettingControls {

    public sealed partial class SaveLocationSettingControl : UserControl {

        private IStorageHelper _storageHelper => App.Services.GetRequiredService<IStorageHelper>();
        private ISettings _settings => App.Services.GetRequiredService<ISettings>();

        #region Public Constructors

        public SaveLocationSettingControl() {
            InitializeComponent();
            InitAsync();
        }

        #endregion Public Constructors

        #region Private Methods

        private async Task InitAsync() {
            StorageFolder folder = await _storageHelper.GetReceiveStorageFolderAsync();
            if (folder != null) {
                PathTextBlock.Text = "Path: " + folder.Path;
            } else {
                PathTextBlock.Text = "Please reselect a folder!";
            }
        }

        private async void MainButton_Click(object sender, RoutedEventArgs e) {
            await SelectFolderAsync();
        }

        private async Task SelectFolderAsync() {
            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            StorageFolder folder = await picker.PickSingleFolderAsync();
            if (folder != null) {
                string folderToken = StorageApplicationPermissions.FutureAccessList.Add(folder);
                _settings.SetSetting(Constants.Receive_SaveToFolder, folderToken);
                PathTextBlock.Text = "Path: " + folder.Path;
            }
        }

        #endregion Private Methods
    }
}