using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Http {

    internal class BeforeHandleRequestResult {
        public HttpServerResponse Response { get; }

        public BeforeHandleRequestResult(HttpServerResponse response) {
            Response = response;
        }
    }
}