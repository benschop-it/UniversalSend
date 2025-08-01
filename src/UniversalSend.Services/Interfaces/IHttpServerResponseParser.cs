using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Interfaces {
    internal interface IHttpServerResponseParser {
        byte[] ConvertToBytes(HttpServerResponse response);
        string ConvertToString(HttpServerResponse response);
    }
}