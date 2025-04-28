using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Models
{
    public class Toast
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
    }

    public class ToastManager
    {
        public event EventHandler SendToastEvent;

        public void SendTosat(Toast toast)
        {
            SendToastEvent?.Invoke(toast,EventArgs.Empty);
        }
    }
}
