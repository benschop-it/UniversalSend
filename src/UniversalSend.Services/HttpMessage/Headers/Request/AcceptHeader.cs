using System.Collections.Generic;
using System.Linq;
using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class AcceptHeader : HttpMultiQuantifiedHeaderBase {
        internal static string NAME = "Accept";

        public IEnumerable<string> AcceptTypes { get; }

        public AcceptHeader(string value, IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues)
            : base(NAME, value, quantifiedHeaderValues) {
            AcceptTypes = QuantifiedHeaderValues.Select(GetMediaType).ToArray();
        }

        private static string GetMediaType(QuantifiedHeaderValue arg) {
            return arg.HeaderValue;
        }

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }
    }
}