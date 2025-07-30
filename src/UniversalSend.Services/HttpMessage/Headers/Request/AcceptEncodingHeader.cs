using System.Collections.Generic;
using System.Linq;
using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class AcceptEncodingHeader : HttpMultiQuantifiedHeaderBase {
        internal static string NAME = "Accept-Encoding";

        public IEnumerable<string> AcceptEncodings { get; }

        public AcceptEncodingHeader(string value, IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues)
            : base(NAME, value, quantifiedHeaderValues) {
            AcceptEncodings = QuantifiedHeaderValues.Select(GetAcceptEncoding).ToArray();
        }

        private static string GetAcceptEncoding(QuantifiedHeaderValue arg) {
            return arg.HeaderValue;
        }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }
    }
}