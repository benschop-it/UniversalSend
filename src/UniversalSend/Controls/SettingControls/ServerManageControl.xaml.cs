using Microsoft.Extensions.DependencyInjection;
using System;
using UniversalSend.Models;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.SettingControls {

    public sealed partial class ServerManageControl : UserControl {

        private readonly ISettings _settings = App.Services.GetRequiredService<ISettings>();
        private readonly IServiceHttpServer _serviceHttpServer = App.Services.GetRequiredService<IServiceHttpServer>();

        #region Public Constructors

        public ServerManageControl() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private async void RestartButton_Click(object sender, RoutedEventArgs e) {
            _serviceHttpServer.StopHttpServer();

            var port = Convert.ToInt32(_settings.GetSettingContentAsString(Constants.Network_Port));

            if (await _serviceHttpServer.StartHttpServerAsync(port)) {
                StopButton.IsEnabled = true;
                RestartButtonIcon.Glyph = "\uE72C";
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e) {
            _serviceHttpServer.StopHttpServer();
            StopButton.IsEnabled = false;
            RestartButtonIcon.Glyph = "\uE768";
        }

        #endregion Private Methods
    }
}