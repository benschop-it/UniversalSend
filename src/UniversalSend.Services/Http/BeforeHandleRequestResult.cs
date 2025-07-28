using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Http {

    public class BeforeHandleRequestResult {
        public HttpServerResponse Response { get; }

        public BeforeHandleRequestResult(HttpServerResponse response) {
            Response = response;
        }
    }
}