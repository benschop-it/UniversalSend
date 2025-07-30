using System.Collections.Generic;

namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class AccessControlAllowHeadersHeader : HttpHeaderBase {
        internal static string NAME = "Access-Control-Allow-Headers";

        public AccessControlAllowHeadersHeader(IEnumerable<string> headers) : base(NAME, string.Join(", ", headers)) {
        }
    }
}