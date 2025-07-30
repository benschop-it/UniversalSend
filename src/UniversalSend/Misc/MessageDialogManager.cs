using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace UniversalSend.Misc {

    public class MessageDialogManager {

        #region Public Methods

        public static async Task<IUICommand> EmptySendTaskAsync() {
            var messageDialog = new MessageDialog("Please select at least one file", "No File Selected");
            return await messageDialog.ShowAsync();
        }

        public static async Task<IUICommand> FileIsNotExistAsync() {
            var messageDialog = new MessageDialog("The file may have been moved, renamed, or deleted", "File Not Found");
            return await messageDialog.ShowAsync();
        }

        #endregion Public Methods
    }
}