using System;
using System.Diagnostics;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.Managers {

    public class SendManager {

        #region Public Events

        // Send task was canceled
        public static event EventHandler SendCanceled;

        // Send task created
        public static event EventHandler SendCreated;

        // Ready to send
        public static event EventHandler SendPrepared;

        // Send request has been sent
        public static event EventHandler SendResuestSended;

        // Sending started
        public static event EventHandler SendStarted;

        // Send state changed
        public static event EventHandler SendStateChanged;

        #endregion Public Events

        #region Public Methods

        public static void SendCanceledEvent() {
            Debug.WriteLine("SendCanceledEvent");
            SendCanceled?.Invoke(null, EventArgs.Empty);
        }

        public static void SendCreatedEvent() {
            Debug.WriteLine("SendCreatedEvent");
            SendCreated?.Invoke(null, EventArgs.Empty);
        }

        public static void SendPreparedEvent(Device device) {
            Debug.WriteLine("SendPreparedEvent");
            SendPrepared?.Invoke(device, EventArgs.Empty);
        }

        public static void SendRequestSendedEvent() {
            Debug.WriteLine("SendRequestSendedEvent");
            SendResuestSended?.Invoke(null, EventArgs.Empty);
        }

        public static void SendStartedEvent() {
            Debug.WriteLine("SendStartedEvent");
            SendStarted?.Invoke(null, EventArgs.Empty);
        }

        public static void SendStateChangedEvent() {
            Debug.WriteLine("SendStateChangedEvent");
            SendStateChanged?.Invoke(null, EventArgs.Empty);
        }

        #endregion Public Methods
    }
}