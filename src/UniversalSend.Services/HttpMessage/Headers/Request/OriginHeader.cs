using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class OriginHeader : HttpRequestHeaderBase {

        #region Internal Fields

        internal static string NAME = "Origin";

        #endregion Internal Fields

        #region Public Constructors

        public OriginHeader(string value) : base(NAME, value) {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }

        #endregion Public Methods
    }
}