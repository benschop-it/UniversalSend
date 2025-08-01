using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Http {

    internal class BeforeHandleRequestResult {

        #region Public Constructors

        public BeforeHandleRequestResult(HttpServerResponse response) {
            Response = response;
        }

        #endregion Public Constructors

        #region Public Properties

        public HttpServerResponse Response { get; }

        #endregion Public Properties
    }
}