using UniversalSend.Models;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Views {

    public sealed partial class ConfirmReceiptPage : Page {

        #region Public Constructors

        public ConfirmReceiptPage() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Properties

        private SendRequestData SendRequestData { get; set; }

        #endregion Private Properties

        #region Private Methods

        private void AcceptButton_Click(object sender, RoutedEventArgs e) {
            ReceiveManager.ChosenOption = true;
            Frame.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            ReceiveManager.ChosenOption = false;
            Frame.GoBack();
        }

        #endregion Private Methods
    }
}