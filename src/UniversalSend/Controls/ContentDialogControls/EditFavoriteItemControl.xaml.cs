using System;
using System.Linq;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Managers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UniversalSend.Controls.ContentDialogControls {

    public sealed partial class EditFavoriteItemControl : UserControl {

        #region Public Constructors

        public EditFavoriteItemControl(Favorite favorite) {
            InitializeComponent();
            Favorite = favorite;

            TitleTextBlock.Text = "Edit";
            DeviceNameTextBox.Text = Favorite.DeviceName;
            IPAddrTextBox.Text = Favorite.IPAddr;
            PortTextBox.Text = Favorite.Port.ToString();
        }

        public EditFavoriteItemControl() {
            InitializeComponent();
            TitleTextBlock.Text = "Add to Favorites";
            DeleteButton.Visibility = Visibility.Collapsed;
        }

        public EditFavoriteItemControl(Device device) {
            InitializeComponent();
            TitleTextBlock.Text = "Add to Favorites";
            DeleteButton.Visibility = Visibility.Collapsed;
            DeviceNameTextBox.Text = device.Alias;
            IPAddrTextBox.Text = device.IP;
            PortTextBox.Text = device.Port.ToString();
        }

        #endregion Public Constructors

        #region Public Properties

        public Favorite Favorite { get; set; }

        #endregion Public Properties

        #region Private Methods

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            ProgramData.ContentDialogManager.HideContentDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) {
            FavoriteManager.Favorites.Remove(Favorite);
            ProgramData.ContentDialogManager.HideContentDialog();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Red);
            /* To-Do: Validate and Save */
            if (string.IsNullOrEmpty(DeviceNameTextBox.Text)) {
                DeviceNameTextBox.BorderBrush = solidColorBrush;
                return;
            }
            if (string.IsNullOrEmpty(IPAddrTextBox.Text) || !StringHelper.IsIpaddr(IPAddrTextBox.Text)) {
                IPAddrTextBox.BorderBrush = solidColorBrush;
                return;
            }
            if (string.IsNullOrEmpty(PortTextBox.Text) && !PortTextBox.Text.All(char.IsDigit)) {
                PortTextBox.BorderBrush = solidColorBrush;
                return;
            } else if (string.IsNullOrEmpty(PortTextBox.Text)) {
                PortTextBox.Text = "53317";
            }

            if (Favorite == null) {
                Favorite = new Favorite(DeviceNameTextBox.Text, IPAddrTextBox.Text, Convert.ToInt64(PortTextBox.Text));
                FavoriteManager.Favorites.Add(Favorite);
                FavoriteManager.SaveFavoritesData();
            } else {
                Favorite.DeviceName = DeviceNameTextBox.Text;
                Favorite.IPAddr = IPAddrTextBox.Text;
                Favorite.Port = Convert.ToInt64(PortTextBox.Text);
                FavoriteManager.SaveFavoritesData();
            }

            ProgramData.ContentDialogManager.HideContentDialog();
        }

        #endregion Private Methods
    }
}