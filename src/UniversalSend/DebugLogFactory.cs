using UniversalSend.Models.Interfaces;

namespace UniversalSend {

    public class DebugLogFactory : ILogFactory {
        private ILogger _debugLogger;

        public DebugLogFactory() {
            _debugLogger = new DebugLogger();
        }

        public void Dispose() {
            _debugLogger = null;
        }

        public ILogger GetLogger(string name) {
            return _debugLogger;
        }

        public ILogger GetLogger<T>() {
            var ns = typeof(T).Namespace;
            var name = typeof(T).Name;
            _debugLogger.SetType(ns + "." + name);
            return _debugLogger;
        }
    }
}