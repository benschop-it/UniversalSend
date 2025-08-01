namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class AccessControlAllowCredentialsHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Access-Control-Allow-Credentials";

        #endregion Internal Fields

        #region Public Constructors

        public AccessControlAllowCredentialsHeader(bool value) : base(NAME, ConvertToString(value)) {
        }

        #endregion Public Constructors

        #region Private Methods

        private static string ConvertToString(bool value) {
            return value.ToString().ToLowerInvariant();
        }

        #endregion Private Methods
    }
}