using UniversalSend.Models;
using UniversalSend.Models.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls {

    public sealed partial class QuickSaveOptionsControl : UserControl {

        #region Public Constructors

        public QuickSaveOptionsControl() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void FavoritesButton_Click(object sender, RoutedEventArgs e) {
            OffButton.IsChecked = OnButton.IsChecked = false;
            FavoritesButton.IsChecked = true;
            ReceiveManager.QuickSave = ReceiveManager.QuickSaveMode.Favorites;
        }

        private void OffButton_Click(object sender, RoutedEventArgs e) {
            FavoritesButton.IsChecked = OnButton.IsChecked = false;
            OffButton.IsChecked = true;
            ReceiveManager.QuickSave = ReceiveManager.QuickSaveMode.Off;
        }

        private void OnButton_Click(object sender, RoutedEventArgs e) {
            FavoritesButton.IsChecked = OffButton.IsChecked = false;
            OnButton.IsChecked = true;
            ReceiveManager.QuickSave = ReceiveManager.QuickSaveMode.On;
        }

        #endregion Private Methods
    }
}