using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Http {

    internal class AfterHandleRequestResult {
        public HttpServerResponse Response { get; }

        public AfterHandleRequestResult(HttpServerResponse response) {
            Response = response;
        }
    }
}