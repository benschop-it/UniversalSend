using UniversalSend.Services.Http;
using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IHttpMessageInspector {

        BeforeHandleRequestResult BeforeHandleRequest(IHttpServerRequest request);

        AfterHandleRequestResult AfterHandleRequest(IHttpServerRequest request, HttpServerResponse httpResponse);
    }
}