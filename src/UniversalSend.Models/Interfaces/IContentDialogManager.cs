using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Models.Interfaces {
    public interface IContentDialogManager {

        #region Public Methods

        Task HideContentDialogAsync();
        Task ShowContentDialogAsync(object Content);
        Task<ContentDialogResult> ShowContentDialogAsync(string Title, string Content, string PrimaryButtonText, string SecondaryButtonText, string CloseButtonText);

        #endregion Public Methods
    }
}