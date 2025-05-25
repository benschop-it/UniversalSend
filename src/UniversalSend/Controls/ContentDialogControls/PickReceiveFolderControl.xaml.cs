using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace UniversalSend.Controls.ContentDialogControls
{
    public sealed partial class PickReceiveFolderControl : UserControl
    {
        public PickReceiveFolderControl()
        {
            this.InitializeComponent();
        }

        private async void OpenPickerButton_Click(object sender, RoutedEventArgs e)
        {
            await PickFolderAsync();
        }

        async Task PickFolderAsync()
        {
            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            StorageFolder folder = await picker.PickSingleFolderAsync();
            if (folder != null)
            {
                string folderToken = StorageApplicationPermissions.FutureAccessList.Add(folder);
                Settings.SetSetting(Settings.Receive_SaveToFolder, folderToken);
                ContentDialogManager.HideContentDialog();
            }
        }
    }
}
