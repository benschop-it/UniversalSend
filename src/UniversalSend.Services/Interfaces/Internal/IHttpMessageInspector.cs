using UniversalSend.Services.Http;
using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IHttpMessageInspector {

        #region Public Methods

        AfterHandleRequestResult AfterHandleRequest(IHttpServerRequest request, HttpServerResponse httpResponse);

        BeforeHandleRequestResult BeforeHandleRequest(IHttpServerRequest request);

        #endregion Public Methods
    }
}