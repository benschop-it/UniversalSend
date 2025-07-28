using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls {

    public sealed partial class SettingsItemControl : UserControl {

        #region Public Constructors

        public SettingsItemControl(string header, FrameworkElement settingControl) {
            InitializeComponent();
            Header = header;
            SettingControl = settingControl;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Header { get; set; }

        public FrameworkElement SettingControl { get; set; }

        #endregion Public Properties

        #region Private Methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            SettingControlGrid.Children.Clear();
            SettingControlGrid.Children.Add(SettingControl);
        }

        #endregion Private Methods
    }
}