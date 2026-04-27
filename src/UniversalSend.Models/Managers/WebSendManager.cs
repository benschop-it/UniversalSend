using System;
using System.Collections.Generic;
using System.Linq;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    internal class WebSendManager : IWebSendManager {

        private readonly object _syncRoot = new object();
        private WebSendSession _activeShare;

        public bool BeginShare(IEnumerable<ISendTaskV2> sendTasks) {
            if (sendTasks == null) {
                return false;
            }

            var tasks = sendTasks.Where(x => x?.File != null).ToList();
            if (tasks.Count == 0) {
                return false;
            }

            var session = new WebSendSession(Guid.NewGuid().ToString("N"), tasks.ToDictionary(x => x.File.Id, x => x));

            lock (_syncRoot) {
                _activeShare = session;
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
            }
        }

        public IWebSendSession GetActiveShare() {
            lock (_syncRoot) {
                return _activeShare;
            }
        }

        private sealed class WebSendSession : IWebSendSession {

            public WebSendSession(string sessionId, IReadOnlyDictionary<string, ISendTaskV2> files) {
                SessionId = sessionId;
                Files = files;
            }

            public IReadOnlyDictionary<string, ISendTaskV2> Files { get; }

            public string SessionId { get; }
        }
    }
}
