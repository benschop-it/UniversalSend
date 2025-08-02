using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using UniversalSend.Misc;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.ContentDialogControls {

    public sealed partial class FavoritesControl : UserControl {

        private IFavoriteManager _favoriteManager => App.Services.GetRequiredService<IFavoriteManager>();
        private ISendTaskManager _sendTaskManager => App.Services.GetRequiredService<ISendTaskManager>();
        private IDeviceManager _deviceManager => App.Services.GetRequiredService<IDeviceManager>();
        private ISendManager _sendManager => App.Services.GetRequiredService<ISendManager>();
        private IContentDialogManager _contentDialogManager => App.Services.GetRequiredService<IContentDialogManager>();

        #region Public Constructors

        public FavoritesControl() {
            InitializeComponent();
            FavoritesListView.ItemsSource = _favoriteManager.Favorites;
        }

        #endregion Public Constructors

        #region Private Methods

        private async void AddButton_Click(object sender, RoutedEventArgs e) {
            await _contentDialogManager.ShowContentDialogAsync(new EditFavoriteItemControl());
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e) {
            await _contentDialogManager.HideContentDialogAsync();
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e) {
            IFavorite favorite = ((Button)sender).DataContext as IFavorite;
            await _contentDialogManager.ShowContentDialogAsync(new EditFavoriteItemControl(favorite));
        }

        private async void FavoritesListView_ItemClick(object sender, ItemClickEventArgs e) {
            await ListViewItemClickAsync((IFavorite)e.ClickedItem);
        }

        private async Task ListViewItemClickAsync(IFavorite item) {
            if (_sendTaskManager.SendTasks.Count == 0) {
                await MessageDialogManager.EmptySendTaskAsync();
                return;
            }

            FindDeviceProgressBar.Visibility = Visibility.Visible;
            IDevice device = await _deviceManager.FindDeviceByIPAsync(item.IPAddr);
            FindDeviceProgressBar.Visibility = Visibility.Collapsed;

            if (device == null) {
                MessageTextBlock.Visibility = Visibility.Visible;
                MessageTextBlock.Text = $"Device not found: {item.DeviceName} ({item.IPAddr})";
            } else {
                _sendManager.SendPreparedEvent(device);
                await _contentDialogManager.HideContentDialogAsync();
            }
        }

        #endregion Private Methods
    }
}