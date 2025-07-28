using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// For more information on the "UserControl" item template, see https://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalSend.Controls.ContentDialogControls {

    public sealed partial class CreateNewFolderControl : UserControl {

        #region Public Constructors

        public CreateNewFolderControl() {
            this.InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void NameTextBox_Tapped(object sender, TappedRoutedEventArgs e) {
            ((TextBox)sender).Focus(FocusState.Pointer);
        }

        #endregion Private Methods
    }
}