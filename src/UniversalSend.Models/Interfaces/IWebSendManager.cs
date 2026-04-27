using System.Collections.Generic;

namespace UniversalSend.Models.Interfaces {

    public interface IWebSendManager {

        bool BeginShare(IEnumerable<ISendTaskV2> sendTasks, string pin = null);

        bool HasActiveShare { get; }

        string GetBrowserDownloadUrl(int port, string ipAddress);

        void ClearShare(string sessionId = null);

        IWebSendSession GetActiveShare();

        /// <summary>
        /// Validates the PIN for a prepare-download request.
        /// Returns true if no PIN is set or the PIN matches.
        /// Tracks attempts and rejects after 3 failures.
        /// </summary>
        WebSendPinResult ValidatePin(string pin);
    }

    public interface IWebSendSession {

        IReadOnlyDictionary<string, ISendTaskV2> Files { get; }

        string SessionId { get; }

        string Pin { get; }
    }

    public enum WebSendPinResult {
        /// <summary>No PIN required, or PIN matched.</summary>
        OK,

        /// <summary>PIN is required but was not provided, or was incorrect.</summary>
        InvalidPin,

        /// <summary>Too many failed attempts from this IP.</summary>
        TooManyAttempts
    }
}
