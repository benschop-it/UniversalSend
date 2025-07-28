using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.ItemControls {

    public sealed partial class StorageItemPropertyItemControl : UserControl {

        #region Public Constructors

        public StorageItemPropertyItemControl(string titleText, string contentText) {
            InitializeComponent();
            TitleText = titleText;
            ContentText = contentText;
        }

        #endregion Public Constructors

        #region Private Properties

        private string ContentText { get; set; }
        private string TitleText { get; set; }

        #endregion Private Properties
    }
}