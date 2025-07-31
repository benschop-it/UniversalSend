using System;
using System.Diagnostics;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    public class SendManager : ISendManager {

        #region Public Events

        // Send task was canceled
        public event EventHandler SendCanceled;

        // Send task created
        public event EventHandler SendCreated;

        // Ready to send
        public event EventHandler SendPrepared;

        // Send request has been sent
        public event EventHandler SendResuestSended;

        // Sending started
        public event EventHandler SendStarted;

        // Send state changed
        public event EventHandler SendStateChanged;

        #endregion Public Events

        #region Public Methods

        public void SendCanceledEvent() {
            Debug.WriteLine("SendCanceledEvent");
            SendCanceled?.Invoke(null, EventArgs.Empty);
        }

        public void SendCreatedEvent() {
            Debug.WriteLine("SendCreatedEvent");
            SendCreated?.Invoke(null, EventArgs.Empty);
        }

        public void SendPreparedEvent(IDevice device) {
            Debug.WriteLine("SendPreparedEvent");
            SendPrepared?.Invoke(device, EventArgs.Empty);
        }

        public void SendRequestSendedEvent() {
            Debug.WriteLine("SendRequestSendedEvent");
            SendResuestSended?.Invoke(null, EventArgs.Empty);
        }

        public void SendStartedEvent() {
            Debug.WriteLine("SendStartedEvent");
            SendStarted?.Invoke(null, EventArgs.Empty);
        }

        public void SendStateChangedEvent() {
            Debug.WriteLine("SendStateChangedEvent");
            SendStateChanged?.Invoke(null, EventArgs.Empty);
        }

        #endregion Public Methods
    }
}