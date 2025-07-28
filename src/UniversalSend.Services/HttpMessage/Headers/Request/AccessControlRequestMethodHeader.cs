using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.HttpMessage.ServerRequestParsers;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    public class AccessControlRequestMethodHeader : HttpRequestHeaderBase {
        internal static string NAME = "Access-Control-Request-Method";

        public HttpMethod Method { get; }

        public AccessControlRequestMethodHeader(string value) : base(NAME, value) {
            Method = HttpMethodParser.GetMethod(value);
        }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }
    }
}