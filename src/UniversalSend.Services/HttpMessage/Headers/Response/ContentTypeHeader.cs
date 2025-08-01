namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class ContentTypeHeader : HttpHeaderBase {

        #region Private Fields

        private const string CHARSET_KEY = "charset";
        private const string NAME = "Content-Type";

        #endregion Private Fields

        #region Public Constructors

        public ContentTypeHeader(string contentType, string charset) :
            base(NAME, FormatContentType(contentType, charset)) {
            Charset = charset;
            ContentType = contentType;
        }

        public ContentTypeHeader(string contentType) : this(contentType, null) {
        }

        #endregion Public Constructors

        #region Public Properties

        public string Charset { get; }
        public string ContentType { get; }

        #endregion Public Properties

        #region Private Methods

        private static string FormatContentType(string contentType, string charset) {
            var charsetPart = string.IsNullOrEmpty(charset) ? string.Empty : $";{CHARSET_KEY}={charset}";
            return string.Concat(contentType, charsetPart);
        }

        #endregion Private Methods
    }
}