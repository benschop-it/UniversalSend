using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class ContentLengthHeader : HttpRequestHeaderBase {

        #region Internal Fields

        internal static string NAME = "Content-Length";

        #endregion Internal Fields

        #region Public Constructors

        public ContentLengthHeader(string value) : base(NAME, value) {
            Length = int.Parse(value);
        }

        #endregion Public Constructors

        #region Public Properties

        public int Length { get; }

        #endregion Public Properties

        #region Public Methods

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }

        #endregion Public Methods
    }
}