namespace UniversalSend.Services.HttpMessage.Models.Contracts {

    internal interface IHttpResponsePartParser {

        string ParseToString(HttpServerResponse response);

        byte[] ParseToBytes(HttpServerResponse response);
    }
}