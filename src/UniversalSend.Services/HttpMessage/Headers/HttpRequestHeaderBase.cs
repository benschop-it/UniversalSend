using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers {

    public abstract class HttpRequestHeaderBase : HttpHeaderBase, IHttpRequestHeader {

        protected HttpRequestHeaderBase(string name, string value) : base(name, value) {
        }

        public abstract void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg);
    }
}