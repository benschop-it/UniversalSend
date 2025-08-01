namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class CloseConnectionHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Connection";

        #endregion Internal Fields

        #region Public Constructors

        public CloseConnectionHeader() : base(NAME, "close") {
        }

        #endregion Public Constructors
    }
}