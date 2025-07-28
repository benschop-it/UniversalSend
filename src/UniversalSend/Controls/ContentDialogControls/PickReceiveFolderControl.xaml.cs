using System;
using System.Threading.Tasks;
using UniversalSend.Models;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.ContentDialogControls {

    public sealed partial class PickReceiveFolderControl : UserControl {

        #region Public Constructors

        public PickReceiveFolderControl() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private async void OpenPickerButton_Click(object sender, RoutedEventArgs e) {
            await PickFolderAsync();
        }

        private async Task PickFolderAsync() {
            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            StorageFolder folder = await picker.PickSingleFolderAsync();
            if (folder != null) {
                string folderToken = StorageApplicationPermissions.FutureAccessList.Add(folder);
                Settings.SetSetting(Settings.Receive_SaveToFolder, folderToken);
                ProgramData.ContentDialogManager.HideContentDialog();
            }
        }

        #endregion Private Methods
    }
}