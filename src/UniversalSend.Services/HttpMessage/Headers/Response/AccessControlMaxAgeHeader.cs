using System;

namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class AccessControlMaxAgeHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Access-Control-Max-Age";

        #endregion Internal Fields

        #region Public Constructors

        public AccessControlMaxAgeHeader(int deltaInSeconds) : base(NAME, Convert.ToString(deltaInSeconds)) {
        }

        #endregion Public Constructors
    }
}