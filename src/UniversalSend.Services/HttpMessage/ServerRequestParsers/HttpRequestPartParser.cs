using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.ServerRequestParsers
{
    internal abstract class HttpRequestPartParser : IHttpRequestPartParser
    {
        public bool IsFinished { get; protected set; }

        public bool IsSucceeded { get; protected set; }

        public byte[] UnparsedData { get; protected set; }

        public abstract void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar);
    }
}
