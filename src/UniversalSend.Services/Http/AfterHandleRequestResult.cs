using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Http {

    internal class AfterHandleRequestResult {

        #region Public Constructors

        public AfterHandleRequestResult(HttpServerResponse response) {
            Response = response;
        }

        #endregion Public Constructors

        #region Public Properties

        public HttpServerResponse Response { get; }

        #endregion Public Properties
    }
}