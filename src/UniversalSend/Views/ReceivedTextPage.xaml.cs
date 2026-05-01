using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using UniversalSend.Misc;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UniversalSend.Views {

    public sealed partial class ReceivedTextPage : Page {

        #region Private Fields

        private bool _responseCompleted;
        private ReceivedTextPageParameter _parameter;

        #endregion Private Fields

        #region Public Constructors

        public ReceivedTextPage() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            IUniversalSendFileV2 file = null;
            string senderName = string.Empty;

            if (e.Parameter is ReceivedTextPageParameter parameter) {
                _parameter = parameter;
                var sendFile = parameter.SendRequestData?.Files?.Values?.FirstOrDefault();
                if (sendFile != null) {
                    file = App.Services.GetRequiredService<IUniversalSendFileManager>().GetUniversalSendFileFromFileRequestDataV2(sendFile);
                }
                senderName = parameter.SendRequestData?.Info?.Alias ?? string.Empty;
            } else if (e.Parameter is IReceiveTask receiveTask) {
                file = receiveTask.FileV2;
                senderName = receiveTask.SenderV2?.Alias ?? string.Empty;
            } else if (e.Parameter is IHistory history) {
                file = history.File;
                senderName = history.Device?.Alias ?? string.Empty;
            } else if (e.Parameter is IUniversalSendFileV2 universalSendFile) {
                file = universalSendFile;
            }

            SenderNameTextBlock.Text = string.IsNullOrWhiteSpace(senderName) ? "Received text message" : senderName;
            ContentTextBox.Text = file?.Preview ?? string.Empty;

            if (IsAbsoluteUri(ContentTextBox.Text)) {
                OpenButton.Visibility = Visibility.Visible;
            } else {
                OpenButton.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            base.OnNavigatedFrom(e);

            if (!_responseCompleted) {
                _parameter?.CompletionSource?.TrySetResult(false);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            CompleteAsAcceptedMessage();
            Frame.GoBack();
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e) {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(ContentTextBox.Text);
            Clipboard.SetContent(dataPackage);
            CompleteAsAcceptedMessage();
            Frame.GoBack();
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e) {
            if (Uri.TryCreate(ContentTextBox.Text?.Trim(), UriKind.Absolute, out var uri)) {
                await Launcher.LaunchUriAsync(uri);
            }
            CompleteAsAcceptedMessage();
            Frame.GoBack();
        }

        private void CompleteAsAcceptedMessage() {
            if (_responseCompleted) {
                return;
            }

            // Returning 204 from prepare-upload is triggered by clearing the file list while still accepting.
            if (_parameter?.SendRequestData?.Files != null) {
                _parameter.SendRequestData.Files.Clear();
            }

            _responseCompleted = true;
            _parameter?.CompletionSource?.TrySetResult(true);
        }

        private static bool IsAbsoluteUri(string text) {
            if (string.IsNullOrWhiteSpace(text) || text.IndexOfAny(new[] { ' ', '\t', '\r', '\n' }) >= 0) {
                return false;
            }

            return Uri.TryCreate(text.Trim(), UriKind.Absolute, out _);
        }

        #endregion Private Methods
    }
}