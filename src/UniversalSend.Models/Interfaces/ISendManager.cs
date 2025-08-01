using System;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.Interfaces {
    public interface ISendManager {

        #region Public Events

        event EventHandler SendCanceled;
        event EventHandler SendCreated;
        event EventHandler SendPrepared;
        event EventHandler SendResuestSended;
        event EventHandler SendStarted;
        event EventHandler SendStateChanged;

        #endregion Public Events

        #region Public Methods

        void SendCanceledEvent();
        void SendCreatedEvent();
        void SendPreparedEvent(IDevice device);
        void SendRequestSendedEvent();
        void SendStartedEvent();
        void SendStateChangedEvent();

        #endregion Public Methods
    }
}