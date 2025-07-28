using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Webserver.Http
{
    public class BeforeHandleRequestResult
    {
        public HttpServerResponse Response { get; }

        public BeforeHandleRequestResult(HttpServerResponse response)
        {
            Response = response;
        }
    }
}