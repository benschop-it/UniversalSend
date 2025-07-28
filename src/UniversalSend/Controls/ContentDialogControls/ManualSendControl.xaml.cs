using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.ContentDialogControls {

    public sealed partial class ManualSendControl : UserControl {

        #region Public Fields

        public int Mode = 0;

        #endregion Public Fields

        #region Public Constructors

        public ManualSendControl() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Mode = MainPivot.SelectedIndex;
        }

        #endregion Private Methods
    }
}