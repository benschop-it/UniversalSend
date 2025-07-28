using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.HttpMessage.Models.Contracts {
    interface IHttpRequestPartParser
    {
        void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar);
        byte[] UnparsedData { get; }
        bool IsFinished { get; }
        bool IsSucceeded { get; }
    }
}
