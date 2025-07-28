using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.ContentDialogControls {

    public sealed partial class FavoritesControl : UserControl {

        #region Public Constructors

        public FavoritesControl() {
            InitializeComponent();
            FavoritesListView.ItemsSource = FavoriteManager.Favorites;
        }

        #endregion Public Constructors

        #region Private Methods

        private async void AddButton_Click(object sender, RoutedEventArgs e) {
            await ProgramData.ContentDialogManager.ShowContentDialogAsync(new EditFavoriteItemControl());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            ProgramData.ContentDialogManager.HideContentDialog();
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e) {
            Favorite favorite = ((Button)sender).DataContext as Favorite;
            await ProgramData.ContentDialogManager.ShowContentDialogAsync(new EditFavoriteItemControl(favorite));
        }

        private async void FavoritesListView_ItemClick(object sender, ItemClickEventArgs e) {
            await ListViewItemClickAsync((Favorite)e.ClickedItem);
        }

        private async Task ListViewItemClickAsync(Favorite item) {
            if (SendTaskManager.SendTasks.Count == 0) {
                await MessageDialogManager.EmptySendTaskAsync();
                return;
            }

            FindDeviceProgressBar.Visibility = Visibility.Visible;
            Device device = await DeviceManager.FindDeviceByIPAsync(item.IPAddr);
            FindDeviceProgressBar.Visibility = Visibility.Collapsed;

            if (device == null) {
                MessageTextBlock.Visibility = Visibility.Visible;
                MessageTextBlock.Text = $"Device not found: {item.DeviceName} ({item.IPAddr})";
            } else {
                SendManager.SendPreparedEvent(device);
                ProgramData.ContentDialogManager.HideContentDialog();
            }
        }

        #endregion Private Methods
    }
}