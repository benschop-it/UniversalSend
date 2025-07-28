using UniversalSend.Models.Data;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls {

    public sealed partial class DeviceItemControl : UserControl {

        #region Public Constructors

        public DeviceItemControl(Device device) {
            InitializeComponent();
            Device = device;

            HashTagTextBlock.Text = $"#{device.IP.Substring(device.IP.LastIndexOf(".") + 1)}";
        }

        #endregion Public Constructors

        #region Private Properties

        private Device Device { get; set; }

        #endregion Private Properties
    }
}