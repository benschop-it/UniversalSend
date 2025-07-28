using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Webserver.Http
{
    internal interface IHttpMessageInspector
    {
        BeforeHandleRequestResult BeforeHandleRequest(IHttpServerRequest request);
        AfterHandleRequestResult AfterHandleRequest(IHttpServerRequest request, HttpServerResponse httpResponse);
    }
}