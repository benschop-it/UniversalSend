using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Tasks;
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

namespace UniversalSend.Controls.ContentDialogControls
{
    public sealed partial class FavoritesControl : UserControl
    {
        public FavoritesControl()
        {
            this.InitializeComponent();
            FavoritesListView.ItemsSource = FavoriteManager.Favorites;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogManager.HideContentDialog();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            await ContentDialogManager.ShowContentDialogAsync(new EditFavoriteItemControl());
        }

        private async void FavoritesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            await ListViewItemClickAsync((Favorite)e.ClickedItem);
        }

        async Task ListViewItemClickAsync(Favorite item)
        {
            if (SendTaskManager.SendTasks.Count == 0)
            {
                await MessageDialogManager.EmptySendTaskAsync();
                return;
            }
            FindDeviceProgressBar.Visibility = Visibility.Visible;
            Device device = await DeviceManager.FindDeviceByIPAsync(item.IPAddr);
            FindDeviceProgressBar.Visibility = Visibility.Collapsed;
            if (device == null)
            {
                MessageTextBlock.Visibility = Visibility.Visible;
                MessageTextBlock.Text = $"未能找到设备：{item.DeviceName}({item.IPAddr})";
            }else
            {
                SendManager.SendPreparedEvent(device);
                ContentDialogManager.HideContentDialog();
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Favorite favorite = ((Button)sender).DataContext as Favorite;
            await ContentDialogManager.ShowContentDialogAsync(new EditFavoriteItemControl(favorite));
        }
    }
}
