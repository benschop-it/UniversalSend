using System;

namespace UniversalSend.Services.Interfaces {
    public enum HttpParseProgressStatus {
        InProgress = 0,
        Finished = 1,
        Error = 2
    }

    public interface ISendRequestProgress {
        long? ContentLength { get; }
        string Error { get; }
        int? Percent { get; }
        long Received { get; }
        HttpParseProgressStatus Status { get; }
        Uri Uri { get; }
    }
}