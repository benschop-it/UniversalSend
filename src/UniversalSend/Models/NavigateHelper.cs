using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Models
{
    public class NavigateHelper
    {
        public static event EventHandler NavigateToHistoryPageEvent;

        public static void NavigateToHitoryPage()
        {
            NavigateToHistoryPageEvent?.Invoke(null,EventArgs.Empty);
        }
    }
}
