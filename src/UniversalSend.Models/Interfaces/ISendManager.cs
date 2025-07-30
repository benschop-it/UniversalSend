using System;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.Interfaces {
    public interface ISendManager {
        event EventHandler SendCanceled;
        event EventHandler SendCreated;
        event EventHandler SendPrepared;
        event EventHandler SendResuestSended;
        event EventHandler SendStarted;
        event EventHandler SendStateChanged;

        void SendCanceledEvent();
        void SendCreatedEvent();
        void SendPreparedEvent(IDevice device);
        void SendRequestSendedEvent();
        void SendStartedEvent();
        void SendStateChangedEvent();
    }
}