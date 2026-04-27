using System;
using System.Collections.Generic;
using System.Linq;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    internal class WebSendManager : IWebSendManager {

        private const int MaxPinAttempts = 3;
        private readonly object _syncRoot = new object();
        private WebSendSession _activeShare;
        private int _pinAttempts;

        public bool BeginShare(IEnumerable<ISendTaskV2> sendTasks, string pin = null) {
            if (sendTasks == null) {
                return false;
            }

            var tasks = sendTasks.Where(x => x?.File != null).ToList();
            if (tasks.Count == 0) {
                return false;
            }

            var session = new WebSendSession(
                Guid.NewGuid().ToString("N"),
                tasks.ToDictionary(x => x.File.Id, x => x),
                string.IsNullOrWhiteSpace(pin) ? null : pin.Trim());

            lock (_syncRoot) {
                _activeShare = session;
                _pinAttempts = 0;
                return true;
            }
        }

        public bool HasActiveShare {
            get {
                lock (_syncRoot) {
                    return _activeShare != null;
                }
            }
        }

        public string GetBrowserDownloadUrl(int port, string ipAddress) {
            if (!HasActiveShare || string.IsNullOrWhiteSpace(ipAddress) || port <= 0) {
                return string.Empty;
            }

            return $"http://{ipAddress}:{port}";
        }

        public void ClearShare(string sessionId = null) {
            lock (_syncRoot) {
                if (_activeShare == null) {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(sessionId) && !string.Equals(_activeShare.SessionId, sessionId, StringComparison.Ordinal)) {
                    return;
                }

                _activeShare = null;
                _pinAttempts = 0;
            }
        }

        public IWebSendSession GetActiveShare() {
            lock (_syncRoot) {
                return _activeShare;
            }
        }

        public WebSendPinResult ValidatePin(string pin) {
            lock (_syncRoot) {
                if (_activeShare == null) {
                    return WebSendPinResult.InvalidPin;
                }

                // No PIN set — always allow
                if (string.IsNullOrEmpty(_activeShare.Pin)) {
                    return WebSendPinResult.OK;
                }

                // Check attempt count first
                if (_pinAttempts >= MaxPinAttempts) {
                    return WebSendPinResult.TooManyAttempts;
                }

                // Correct PIN
                if (string.Equals(pin, _activeShare.Pin, StringComparison.Ordinal)) {
                    return WebSendPinResult.OK;
                }

                // Wrong PIN — only count as an attempt if something was actually submitted
                if (!string.IsNullOrEmpty(pin)) {
                    _pinAttempts++;

                    if (_pinAttempts >= MaxPinAttempts) {
                        return WebSendPinResult.TooManyAttempts;
                    }
                }

                return WebSendPinResult.InvalidPin;
            }
        }

        private sealed class WebSendSession : IWebSendSession {

            public WebSendSession(string sessionId, IReadOnlyDictionary<string, ISendTaskV2> files, string pin) {
                SessionId = sessionId;
                Files = files;
                Pin = pin;
            }

            public IReadOnlyDictionary<string, ISendTaskV2> Files { get; }

            public string SessionId { get; }

            public string Pin { get; }
        }
    }
}
