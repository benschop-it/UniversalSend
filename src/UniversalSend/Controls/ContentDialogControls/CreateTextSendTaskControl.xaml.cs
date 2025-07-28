using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.ContentDialogControls {

    public sealed partial class CreateTextSendTaskControl : UserControl {

        #region Public Constructors

        public CreateTextSendTaskControl() {
            this.InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public string GetText() {
            return MainTextBox.Text;
        }

        #endregion Public Methods
    }
}