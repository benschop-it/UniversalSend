using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UniversalSend.Controls.ContentDialogControls {

    public sealed partial class EditFavoriteItemControl : UserControl {

        private IFavoriteManager _favoriteManager => App.Services.GetRequiredService<IFavoriteManager>();
        private IContentDialogManager _contentDialogManager => App.Services.GetRequiredService<IContentDialogManager>();

        #region Public Constructors

        public EditFavoriteItemControl(IFavorite favorite) {
            InitializeComponent();
            Favorite = favorite;

            TitleTextBlock.Text = "Edit";
            DeviceNameTextBox.Text = favorite.DeviceName;
            IPAddrTextBox.Text = favorite.IPAddr;
            PortTextBox.Text = favorite.Port.ToString();
        }

        public EditFavoriteItemControl() {
            InitializeComponent();

            TitleTextBlock.Text = "Add to Favorites";
            DeleteButton.Visibility = Visibility.Collapsed;
        }

        public EditFavoriteItemControl(IDevice device) {
            InitializeComponent();
            TitleTextBlock.Text = "Add to Favorites";
            DeleteButton.Visibility = Visibility.Collapsed;
            DeviceNameTextBox.Text = device.Alias;
            IPAddrTextBox.Text = device.IP;
            PortTextBox.Text = device.Port.ToString();
        }

        #endregion Public Constructors

        #region Public Properties

        public IFavorite Favorite { get; set; }

        #endregion Public Properties

        #region Private Methods

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            _contentDialogManager.HideContentDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e) {
            _favoriteManager.Favorites.Remove(Favorite);
            _contentDialogManager.HideContentDialog();
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
                Favorite = _favoriteManager.CreateFavorite(DeviceNameTextBox.Text, IPAddrTextBox.Text, Convert.ToInt64(PortTextBox.Text));
                _favoriteManager.Favorites.Add(Favorite);
                _favoriteManager.SaveFavoritesData();
            } else {
                Favorite.DeviceName = DeviceNameTextBox.Text;
                Favorite.IPAddr = IPAddrTextBox.Text;
                Favorite.Port = Convert.ToInt64(PortTextBox.Text);
                _favoriteManager.SaveFavoritesData();
            }

            _contentDialogManager.HideContentDialog();
        }

        #endregion Private Methods
    }
}