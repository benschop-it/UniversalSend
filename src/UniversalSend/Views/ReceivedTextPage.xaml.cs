using Microsoft.Extensions.DependencyInjection;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UniversalSend.Views {

    public sealed partial class ReceivedTextPage : Page {

        #region Public Constructors

        public ReceivedTextPage() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            IUniversalSendFileV2 file = null;
            string senderName = string.Empty;

            if (e.Parameter is IReceiveTask receiveTask) {
                file = receiveTask.FileV2;
                senderName = receiveTask.SenderV2?.Alias ?? string.Empty;
            } else if (e.Parameter is IHistory history) {
                file = history.File;
                senderName = history.Device?.Alias ?? string.Empty;
            } else if (e.Parameter is IUniversalSendFileV2 universalSendFile) {
                file = universalSendFile;
            }

            SenderNameTextBlock.Text = string.IsNullOrWhiteSpace(senderName) ? "Received text message" : senderName;
            ContentTextBox.Text = file?.Preview ?? string.Empty;
        }

        #endregion Protected Methods

        #region Private Methods

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e) {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(ContentTextBox.Text);
            Clipboard.SetContent(dataPackage);
        }

        #endregion Private Methods
    }
}