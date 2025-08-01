using System.Collections.Generic;
using UniversalSend.Services.HttpMessage.Models.Schemas;

namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class AccessControlAllowMethodsHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Access-Control-Allow-Methods";

        #endregion Internal Fields

        #region Public Constructors

        public AccessControlAllowMethodsHeader(IEnumerable<HttpMethod> methods) : base(NAME, string.Join(", ", methods)) {
        }

        #endregion Public Constructors
    }
}