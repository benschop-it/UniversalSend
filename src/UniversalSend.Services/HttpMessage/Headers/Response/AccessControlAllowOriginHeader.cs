namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class AccessControlAllowOriginHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Access-Control-Allow-Origin";

        #endregion Internal Fields

        #region Public Constructors

        public AccessControlAllowOriginHeader(string value) : base(NAME, value) {
        }

        #endregion Public Constructors
    }
}