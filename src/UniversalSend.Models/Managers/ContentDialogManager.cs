using System;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Models.Managers {

    internal class ContentDialogManager : IContentDialogManager {

        #region Private Properties

        private ContentDialog _contentDialog { get; set; } = new ContentDialog();

        private bool _hidden { get; set; } = true;

        #endregion Private Properties

        #region Public Methods

        public void HideContentDialog() {
            _contentDialog.Hide();
        }

        public async Task ShowContentDialogAsync(object Content) {
            _contentDialog.Hide();
            _contentDialog = new ContentDialog();
            _contentDialog.Content = Content;
            await _contentDialog.ShowAsync();
        }

        public async Task<ContentDialogResult> ShowContentDialogAsync(
            string Title,
            string Content,
            string PrimaryButtonText,
            string SecondaryButtonText,
            string CloseButtonText
        ) {
            _contentDialog.Hide();
            _contentDialog = new ContentDialog();
            _contentDialog.Title = Title;
            _contentDialog.Content = Content;
            if (string.IsNullOrEmpty(SecondaryButtonText)) {
                _contentDialog.PrimaryButtonText = PrimaryButtonText;
                _contentDialog.DefaultButton = ContentDialogButton.Primary;
            }
            if (string.IsNullOrEmpty(SecondaryButtonText))
                _contentDialog.SecondaryButtonText = SecondaryButtonText;
            _contentDialog.CloseButtonText = CloseButtonText;

            return await _contentDialog.ShowAsync();
        }

        #endregion Public Methods

    }
}