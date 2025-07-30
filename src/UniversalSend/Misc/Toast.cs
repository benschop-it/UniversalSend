using System;

namespace UniversalSend.Misc {

    public class Toast {

        #region Public Properties

        public string Content { get; set; } = "";
        public string Title { get; set; } = "";

        #endregion Public Properties
    }

    public class ToastManager {

        #region Public Events

        public event EventHandler SendToastEvent;

        #endregion Public Events

        #region Public Methods

        public void SendTosat(Toast toast) {
            SendToastEvent?.Invoke(toast, EventArgs.Empty);
        }

        #endregion Public Methods
    }
}