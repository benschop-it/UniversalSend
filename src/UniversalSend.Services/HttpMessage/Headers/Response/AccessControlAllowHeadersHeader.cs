using System.Collections.Generic;

namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class AccessControlAllowHeadersHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Access-Control-Allow-Headers";

        #endregion Internal Fields

        #region Public Constructors

        public AccessControlAllowHeadersHeader(IEnumerable<string> headers) : base(NAME, string.Join(", ", headers)) {
        }

        #endregion Public Constructors
    }
}