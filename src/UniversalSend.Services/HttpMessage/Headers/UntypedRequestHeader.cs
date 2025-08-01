using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers {

    internal class UntypedRequestHeader : HttpRequestHeaderBase {

        #region Public Constructors

        public UntypedRequestHeader(string name, string value) : base(name, value) {
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }

        #endregion Public Methods
    }
}