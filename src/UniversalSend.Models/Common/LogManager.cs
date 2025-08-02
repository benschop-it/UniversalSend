using System;
using System.Threading;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Common {

    public static class LogManager {

        #region Private Fields

        private static ILogFactory _logFactory;

        #endregion Private Fields

        #region Public Constructors

        static LogManager() {
            _logFactory = new NullLogFactory();
        }

        #endregion Public Constructors

        #region Public Methods

        public static ILogger GetLogger<T>() {
            return _logFactory.GetLogger<T>();
        }

        public static ILogger GetLogger(string name) {
            return _logFactory.GetLogger(name);
        }

        public static void SetLogFactory(ILogFactory logFactory) {
            Interlocked.Exchange(ref _logFactory, logFactory);
        }

        #endregion Public Methods

        #region Private Classes

        private class NullLogFactory : ILogFactory {

            #region Private Fields

            private readonly NullLogger _nullLogger;

            #endregion Private Fields

            #region Public Constructors

            public NullLogFactory() {
                _nullLogger = new NullLogger();
            }

            #endregion Public Constructors

            #region Public Methods

            public void Dispose() {
            }

            ILogger ILogFactory.GetLogger<T>() {
                return _nullLogger;
            }

            ILogger ILogFactory.GetLogger(string name) {
                return _nullLogger;
            }

            #endregion Public Methods
        }

        private class NullLogger : AbstractLogger {

            #region Protected Methods

            protected override bool IsLogEnabled(LogLevel trace) => false;

            protected override void LogMessage(string message, LogLevel loggingLevel, params object[] args) {
            }

            protected override void LogMessage(string message, LogLevel loggingLevel, Exception ex) {
            }

            protected override void SetLogType(string logType) {
            }

            #endregion Protected Methods
        }

        #endregion Private Classes
    }
}