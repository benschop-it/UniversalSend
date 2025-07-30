using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers {

    internal abstract class HttpRequestHeaderBase : HttpHeaderBase, IHttpRequestHeader {

        protected HttpRequestHeaderBase(string name, string value) : base(name, value) {
        }

        public abstract void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg);
    }
}