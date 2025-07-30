namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class CloseConnectionHeader : HttpHeaderBase {
        internal static string NAME = "Connection";

        public CloseConnectionHeader() : base(NAME, "close") {
        }
    }
}