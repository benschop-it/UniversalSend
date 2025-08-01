using System;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Common {

    /// <summary>
    /// Implementing providers can use this implementation to avoid a bit of boilerplate.
    /// </summary>
    public abstract class AbstractLogger : ILogger {

        #region Protected Enums

        protected enum LogLevel {
            Trace,
            Debug,
            Info,
            Warn,
            Error,
            Fatal
        }

        #endregion Protected Enums

        #region Public Methods

        public void Debug(string message, Exception ex) => LogMessage(message, LogLevel.Debug, ex);

        public void Debug(string message, params object[] args) => LogMessage(message, LogLevel.Debug, args);

        public void Error(string message, Exception ex) => LogMessage(message, LogLevel.Error, ex);

        public void Error(string message, params object[] args) => LogMessage(message, LogLevel.Error, args);

        public void Fatal(string message, Exception ex) => LogMessage(message, LogLevel.Fatal, ex);

        public void Fatal(string message, params object[] args) => LogMessage(message, LogLevel.Fatal, args);

        public void Info(string message, Exception ex) => LogMessage(message, LogLevel.Info, ex);

        public void Info(string message, params object[] args) => LogMessage(message, LogLevel.Info, args);

        public bool IsDebugEnabled() => IsLogEnabled(LogLevel.Debug);

        public bool IsErrorEnabled() => IsLogEnabled(LogLevel.Error);

        public bool IsFatalEnabled() => IsLogEnabled(LogLevel.Fatal);

        public bool IsInfoEnabled() => IsLogEnabled(LogLevel.Info);

        public bool IsLogEnabled() => IsLogEnabled(LogLevel.Trace);

        public bool IsWarnEnabled() => IsLogEnabled(LogLevel.Warn);

        public void Trace(string message, Exception ex) => LogMessage(message, LogLevel.Trace, ex);

        public void Trace(string message, params object[] args) => LogMessage(message, LogLevel.Trace, args);
        public void Warn(string message, Exception ex) => LogMessage(message, LogLevel.Warn, ex);

        public void Warn(string message, params object[] args) => LogMessage(message, LogLevel.Warn, args);

        #endregion Public Methods

        #region Protected Methods

        protected abstract bool IsLogEnabled(LogLevel trace);

        protected abstract void LogMessage(string message, LogLevel loggingLevel, params object[] args);

        protected abstract void LogMessage(string message, LogLevel loggingLevel, Exception ex);

        #endregion Protected Methods
    }
}