using System;

namespace UniversalSend.Models {

    public class NavigateHelper {

        #region Public Events

        public static event EventHandler NavigateToHistoryPageEvent;

        #endregion Public Events

        #region Public Methods

        public static void NavigateToHistoryPage() {
            NavigateToHistoryPageEvent?.Invoke(null, EventArgs.Empty);
        }

        #endregion Public Methods
    }
}