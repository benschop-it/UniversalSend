using Microsoft.Extensions.DependencyInjection;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls {

    public sealed partial class QuickSaveOptionsControl : UserControl {

        private IReceiveManager _receiveManager => App.Services.GetRequiredService<IReceiveManager>();


        #region Public Constructors

        public QuickSaveOptionsControl() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void FavoritesButton_Click(object sender, RoutedEventArgs e) {
            OffButton.IsChecked = OnButton.IsChecked = false;
            FavoritesButton.IsChecked = true;
            _receiveManager.QuickSave = QuickSaveMode.Favorites;
        }

        private void OffButton_Click(object sender, RoutedEventArgs e) {
            FavoritesButton.IsChecked = OnButton.IsChecked = false;
            OffButton.IsChecked = true;
            _receiveManager.QuickSave = QuickSaveMode.Off;
        }

        private void OnButton_Click(object sender, RoutedEventArgs e) {
            FavoritesButton.IsChecked = OffButton.IsChecked = false;
            OnButton.IsChecked = true;
            _receiveManager.QuickSave = QuickSaveMode.On;
        }

        #endregion Private Methods
    }
}