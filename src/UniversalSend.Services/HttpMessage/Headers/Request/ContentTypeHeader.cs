using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class ContentTypeHeader : HttpSingleQuantifiedHeaderBase {

        #region Internal Fields

        internal static string CHARSET_QUANTIFIER_NAME = "charset";
        internal static string NAME = "Content-Type";

        #endregion Internal Fields

        #region Public Constructors

        public ContentTypeHeader(string value, QuantifiedHeaderValue quantifiedHeaderValue)
            : base(NAME, value, quantifiedHeaderValue) {
            ContentType = QuantifiedHeaderValue.HeaderValue;
            ContentCharset = QuantifiedHeaderValue.FindQuantifierValue(CHARSET_QUANTIFIER_NAME);
        }

        #endregion Public Constructors

        #region Public Properties

        public string ContentCharset { get; }
        public string ContentType { get; }

        #endregion Public Properties

        #region Public Methods

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }

        #endregion Public Methods
    }
}