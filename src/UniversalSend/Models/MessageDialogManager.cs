using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace UniversalSend.Models
{
    public class MessageDialogManager
    {
        public static async Task<IUICommand> EmptySendTaskAsync()
        {
            var messageDialog = new MessageDialog("请至少选择一个文件","未选择文件");
            return await messageDialog.ShowAsync();
        }

        public static async Task<IUICommand> FileIsNotExistAsync()
        {
            var messageDialog = new MessageDialog("文件可能被移动、改名或删除", "找不到该文件");
            return await messageDialog.ShowAsync();
        }
    }
}
