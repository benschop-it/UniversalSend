using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Models
{
    public class ContentDialogManager
    {

        ContentDialog ContentDialog { get; set; } = new ContentDialog();

        bool Hided { get; set; } = true;
        public async Task ShowContentDialogAsync(object Content)
        {
            ContentDialog.Hide();
            ContentDialog = new ContentDialog();
            ContentDialog.Content = Content;
            await ContentDialog.ShowAsync();
        }

        public async Task<ContentDialogResult> ShowContentDialogAsync(string Title, string Content, string PrimaryButtonText, string SecondaryButtonText, string CloseButtonText)
        {
            ContentDialog.Hide();
            ContentDialog = new ContentDialog();
            ContentDialog.Title = Title;
            ContentDialog.Content = Content;
            if (String.IsNullOrEmpty(SecondaryButtonText))
            {
                ContentDialog.PrimaryButtonText = PrimaryButtonText;
                ContentDialog.DefaultButton = ContentDialogButton.Primary;
            }
            if (String.IsNullOrEmpty(SecondaryButtonText))
                ContentDialog.SecondaryButtonText = SecondaryButtonText;
            ContentDialog.CloseButtonText = CloseButtonText;


            return await ContentDialog.ShowAsync();
        }

        public void HideContentDialog()
        {
            ContentDialog.Hide();
        }

        //public async Task ShowPickReceiveFolderDialogAsync()
        //{
        //    await ShowContentDialogAsync(new PickReceiveFolderControl());
        //}
    }
}
