using System;
using UniversalSend.Services.Logging;

namespace UniversalSend {

    public class DebugLogger : AbstractLogger {

        protected override bool IsLogEnabled(LogLevel trace) {
            // Ignore level, log everything
            return true;
        }

        protected override void LogMessage(string message, LogLevel loggingLevel, Exception ex) {
            System.Diagnostics.Debug.WriteLine($"{loggingLevel}: {message}");
            System.Diagnostics.Debug.WriteLine($"{ex}");
        }

        protected override void LogMessage(string message, LogLevel loggingLevel, params object[] args) {
            System.Diagnostics.Debug.WriteLine($"{loggingLevel}: {(string.Format(message, args))}");
        }
    }
}