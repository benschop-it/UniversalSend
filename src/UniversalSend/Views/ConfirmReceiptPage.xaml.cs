using Microsoft.Extensions.DependencyInjection;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Views {

    public sealed partial class ConfirmReceiptPage : Page {

        private IReceiveManager _receiveManager => App.Services.GetRequiredService<IReceiveManager>();

        #region Public Constructors

        public ConfirmReceiptPage() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Properties

        private ISendRequestDataV2 SendRequestDataV2 { get; set; }

        #endregion Private Properties

        #region Private Methods

        private void AcceptButton_Click(object sender, RoutedEventArgs e) {
            _receiveManager.ChosenOption = true;
            Frame.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            _receiveManager.ChosenOption = false;
            Frame.GoBack();
        }

        #endregion Private Methods
    }
}