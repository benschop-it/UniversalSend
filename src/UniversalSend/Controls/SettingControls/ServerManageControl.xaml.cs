using System;
using UniversalSend.Models;
using UniversalSend.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.SettingControls {

    public sealed partial class ServerManageControl : UserControl {

        #region Public Constructors

        public ServerManageControl() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private async void RestartButton_Click(object sender, RoutedEventArgs e) {
            ServiceHttpServer server = ProgramData.ServiceServer as ServiceHttpServer;

            server.StopHttpServer();

            var port = Convert.ToInt32(Settings.GetSettingContentAsString(Settings.Network_Port));

            if (await server.StartHttpServerAsync(port)) {
                StopButton.IsEnabled = true;
                RestartButtonIcon.Glyph = "\uE72C";
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            ServiceHttpServer server = ProgramData.ServiceServer as ServiceHttpServer;

            server.StopHttpServer();
            StopButton.IsEnabled = false;
            RestartButtonIcon.Glyph = "\uE768";
        }

        #endregion Private Methods
    }
}