using System;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Models.Managers {

    internal class ContentDialogManager : IContentDialogManager {

        #region Private Properties

        private ContentDialog _contentDialog { get; set; } = new ContentDialog();

        private bool _hidden { get; set; } = true;

        #endregion Private Properties

        #region Public Methods

        public async Task HideContentDialogAsync() {
            if (_contentDialog == null) return;

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, () => _contentDialog.Hide());
        }

        public async Task ShowContentDialogAsync(object Content) {
            var tcs = new TaskCompletionSource<bool>();

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, async () => {
                    try {
                        _contentDialog.Hide();
                        _contentDialog = new ContentDialog();
                        _contentDialog.Content = Content;
                        await _contentDialog.ShowAsync();
                        tcs.TrySetResult(true);
                    } catch (Exception ex) {
                        tcs.TrySetException(ex);
                    }
                });

            await tcs.Task;
        }

        public async Task<ContentDialogResult> ShowContentDialogAsync(
            string Title,
            string Content,
            string PrimaryButtonText,
            string SecondaryButtonText,
            string CloseButtonText
        ) {
            var tcs = new TaskCompletionSource<ContentDialogResult>();

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, async () => {
                    _contentDialog.Hide();
                    _contentDialog = new ContentDialog();
                    _contentDialog.Title = Title;
                    _contentDialog.Content = Content;

                    if (!string.IsNullOrEmpty(PrimaryButtonText)) {
                        _contentDialog.PrimaryButtonText = PrimaryButtonText;
                        _contentDialog.DefaultButton = ContentDialogButton.Primary;
                    }

                    if (!string.IsNullOrEmpty(SecondaryButtonText)) {
                        _contentDialog.SecondaryButtonText = SecondaryButtonText;
                    }

                    if (!string.IsNullOrEmpty(CloseButtonText)) {
                        _contentDialog.CloseButtonText = CloseButtonText;
                    }

                    tcs.TrySetResult(await _contentDialog.ShowAsync());
                });

            return await tcs.Task;
        }

        #endregion Public Methods

    }
}