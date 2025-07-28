using Restup.WebServer.Logging;

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
            return _debugLogger;
        }
    }
}