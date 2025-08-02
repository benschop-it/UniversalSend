using System;
using UniversalSend.Models.Common;

namespace UniversalSend {

    public class DebugLogger : AbstractLogger {
        private string _logType = "[unknown]";

        protected override bool IsLogEnabled(LogLevel trace) {
            // Ignore level, log everything
            return true;
        }

        protected override void LogMessage(string message, LogLevel loggingLevel, Exception ex) {
            System.Diagnostics.Debug.WriteLine($"{_logType} {loggingLevel}: {message}");
            System.Diagnostics.Debug.WriteLine($"{_logType} {ex}");
        }

        protected override void LogMessage(string message, LogLevel loggingLevel, params object[] args) {
            string formattedString = message;
            if (args.Length > 0) {
                 formattedString = (string.Format(message, args));
            }
            System.Diagnostics.Debug.WriteLine($"{_logType} {loggingLevel}: {formattedString}");
        }

        protected override void SetLogType(string logType) {
            _logType = $"[{logType}]";
        }
    }
}