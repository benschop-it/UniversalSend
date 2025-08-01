namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class ContentEncodingHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Content-Encoding";

        #endregion Internal Fields

        #region Public Constructors

        public ContentEncodingHeader(string encoding) : base(NAME, encoding) {
            Encoding = encoding;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Encoding { get; }

        #endregion Public Properties
    }
}