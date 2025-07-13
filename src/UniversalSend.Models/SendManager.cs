using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;

namespace UniversalSend.Models
{
    public class SendManager
    {
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

        // Send task was canceled
        public static event EventHandler SendCanceled;

        public static void SendCreatedEvent()
        {
            SendCreated?.Invoke(null, EventArgs.Empty);
        }

        public static void SendPreparedEvent(Device device)
        {
            SendPrepared?.Invoke(device, EventArgs.Empty);
        }

        public static void SendRequestSendedEvent()
        {
            SendResuestSended?.Invoke(null, EventArgs.Empty);
        }

        public static void SendStateChangedEvent()
        {
            SendStateChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void SendStartedEvent()
        {
            SendStarted?.Invoke(null, EventArgs.Empty);
        }

        public static void SendCanceledEvent()
        {
            SendCanceled?.Invoke(null, EventArgs.Empty);
        }
    }
}
