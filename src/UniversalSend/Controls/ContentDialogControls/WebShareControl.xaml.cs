using Microsoft.Extensions.DependencyInjection;
using System;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.ContentDialogControls {

    public sealed partial class WebShareControl : UserControl {

        private bool _isLoaded;
        private readonly ISettings _settings = App.Services.GetRequiredService<ISettings>();
        private readonly IWebSendManager _webSendManager = App.Services.GetRequiredService<IWebSendManager>();
        private readonly ISendTaskManager _sendTaskManager = App.Services.GetRequiredService<ISendTaskManager>();
        private readonly IContentDialogManager _contentDialogManager = App.Services.GetRequiredService<IContentDialogManager>();

        private string _currentPin;

        public WebShareControl() {
            InitializeComponent();
        }

        /// <summary>Raised when the user taps "Stop sharing".</summary>
        public event EventHandler StopSharing;

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            // Show the URL
            UrlTextBlock.Text = _sendTaskManager.LastWebShareUrl ?? string.Empty;

            // Load PIN settings
            var requirePinSetting = _settings.GetSettingContent(Constants.WebShare_RequirePin);
            bool requirePin = requirePinSetting is bool b ? b : bool.TryParse(requirePinSetting?.ToString(), out bool parsed) && parsed;

            _currentPin = _sendTaskManager.LastWebSharePin;

            RequirePinToggle.IsOn = requirePin;
            _isLoaded = true;
            UpdatePinDisplay();
        }

        private void CopyUrlButton_Click(object sender, RoutedEventArgs e) {
            var dataPackage = new DataPackage();
            dataPackage.SetText(UrlTextBlock.Text);
            Clipboard.SetContent(dataPackage);
        }

        private void AutoAcceptToggle_Toggled(object sender, RoutedEventArgs e) {
            // Auto-accept is a UI-only hint for now; the session doesn't enforce it yet.
        }

        private void RequirePinToggle_Toggled(object sender, RoutedEventArgs e) {
            if (!_isLoaded || RequirePinToggle == null) {
                return;
            }

            bool requirePin = RequirePinToggle.IsOn;
            _settings.SetSetting(Constants.WebShare_RequirePin, requirePin);

            if (requirePin) {
                // Ensure we have a PIN
                if (string.IsNullOrWhiteSpace(_currentPin)) {
                    _currentPin = new Random().Next(100000, 999999).ToString();
                    _settings.SetSetting(Constants.WebShare_Pin, _currentPin);
                }
            }

            // Re-publish the share with updated PIN
            RepublishShare();
            UpdatePinDisplay();
        }

        private async void ChangePinButton_Click(object sender, RoutedEventArgs e) {
            // Hide the current content dialog first (UWP only allows one at a time)
            await _contentDialogManager.HideContentDialogAsync();

            var inputBox = new TextBox {
                PlaceholderText = "Enter PIN",
                Text = _currentPin ?? string.Empty,
                MaxLength = 16,
                Margin = new Thickness(0, 8, 0, 0)
            };

            var dialog = new ContentDialog {
                Title = "Enter PIN",
                Content = inputBox,
                PrimaryButtonText = "Confirm",
                SecondaryButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(inputBox.Text)) {
                _currentPin = inputBox.Text.Trim();
                _settings.SetSetting(Constants.WebShare_Pin, _currentPin);
                RepublishShare();
            }

            // Re-show the web share dialog
            var newControl = new WebShareControl();
            newControl.StopSharing += (s, args) => {
                _contentDialogManager.HideContentDialogAsync();
            };
            await _contentDialogManager.ShowContentDialogAsync(newControl);
        }

        private void StopSharingButton_Click(object sender, RoutedEventArgs e) {
            _sendTaskManager.ClearWebShare();
            StopSharing?.Invoke(this, EventArgs.Empty);
        }

        private void RepublishShare() {
            // Re-create the share with the current PIN setting
            bool requirePin = RequirePinToggle.IsOn;
            string pin = requirePin ? _currentPin : null;
            _webSendManager.BeginShare(_sendTaskManager.SendTasksV2, pin);
        }

        private void UpdatePinDisplay() {
            if (RequirePinToggle == null || PinDisplayPanel == null || PinValueTextBlock == null) {
                return;
            }

            bool showPin = RequirePinToggle.IsOn && !string.IsNullOrWhiteSpace(_currentPin);
            PinDisplayPanel.Visibility = showPin ? Visibility.Visible : Visibility.Collapsed;
            PinValueTextBlock.Text = _currentPin ?? string.Empty;
        }
    }
}
