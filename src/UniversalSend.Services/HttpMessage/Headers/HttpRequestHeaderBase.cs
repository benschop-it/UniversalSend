using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers {

    internal abstract class HttpRequestHeaderBase : HttpHeaderBase, IHttpRequestHeader {

        #region Protected Constructors

        protected HttpRequestHeaderBase(string name, string value) : base(name, value) {
        }

        #endregion Protected Constructors

        #region Public Methods

        public abstract void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg);

        #endregion Public Methods
    }
}