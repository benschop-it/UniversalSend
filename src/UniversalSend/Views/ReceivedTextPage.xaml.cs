using UniversalSend.Models.Data;
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
            UniversalSendFile file = (UniversalSendFile)e.Parameter;
            SenderNameTextBlock.Text = "";
            ContentTextBox.Text = file.Text;
        }

        #endregion Protected Methods

        #region Private Methods

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e) {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(ContentTextBox.Text);
        }

        #endregion Private Methods
    }
}