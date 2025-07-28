using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.SendPage {

    public sealed partial class SendItemButtonControl : UserControl {

        #region Public Constructors

        public SendItemButtonControl(string iconGlyph, string label) {
            this.InitializeComponent();
            IconGlyph = iconGlyph;
            Label = label;
        }

        #endregion Public Constructors

        #region Public Properties

        public string IconGlyph { get; set; }

        public string Label { get; set; }

        #endregion Public Properties
    }
}