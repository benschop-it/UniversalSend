using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Interfaces.Internal {
    internal interface IHttpServerResponseParser {
        byte[] ConvertToBytes(HttpServerResponse response);
        string ConvertToString(HttpServerResponse response);
    }
}