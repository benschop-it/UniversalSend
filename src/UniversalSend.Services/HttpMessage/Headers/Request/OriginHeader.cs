using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers.Request {
    public class OriginHeader : HttpRequestHeaderBase
    {
        internal static string NAME = "Origin";

        public OriginHeader(string value) : base(NAME, value)
        {
        }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg)
        {
            v.Visit(this, arg);
        }
    }
}