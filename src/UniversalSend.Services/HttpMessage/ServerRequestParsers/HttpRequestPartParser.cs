using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.ServerRequestParsers {

    internal abstract class HttpRequestPartParser : IHttpRequestPartParser {
        public bool IsFinished { get; protected set; }

        public bool IsSucceeded { get; protected set; }

        public byte[] UnparsedData { get; protected set; }

        public abstract void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar);
    }
}