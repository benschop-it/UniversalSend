using System;

namespace UniversalSend.Services.Interfaces {
    // C# 7.3: no records; keep it simple and immutable

    public enum HttpParseProgressStatus {
        InProgress = 0,
        Finished = 1,
        Error = 2
    }

    public sealed class HttpParseProgressEventArgs : EventArgs {
        public HttpParseProgressEventArgs(
            HttpParseProgressStatus status,
            int? percent,
            long received,
            long? contentLength,
            Uri uri,
            string error = null
        ) {
            Status = status;
            Percent = percent;
            Received = received;
            ContentLength = contentLength;
            Uri = uri;
            Error = error;
        }

        public HttpParseProgressStatus Status { get; }

        /// <summary>
        /// 0..100 when content length is known; null when unknown.
        /// </summary>
        public int? Percent { get; }

        /// <summary>
        /// Total bytes received so far.
        /// </summary>
        public long Received { get; }

        /// <summary>
        /// Total content length if known; otherwise null.
        /// </summary>
        public long? ContentLength { get; }

        /// <summary>
        /// Request URI for correlation when multiple requests are in flight.
        /// </summary>
        public Uri Uri { get; }

        public string Error { get; }
    }
}
