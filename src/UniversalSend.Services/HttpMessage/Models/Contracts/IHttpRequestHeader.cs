namespace UniversalSend.Services.HttpMessage.Models.Contracts {

    internal interface IHttpRequestHeader : IHttpHeader {

        void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg);
    }
}