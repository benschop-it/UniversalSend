using System.Collections.Generic;
using System.Linq;
using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class AcceptCharsetHeader : HttpMultiQuantifiedHeaderBase {

        #region Internal Fields

        internal static string NAME = "Accept-Charset";

        #endregion Internal Fields

        #region Public Constructors

        public AcceptCharsetHeader(string value, IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues)
            : base(NAME, value, quantifiedHeaderValues) {
            ResponseContentEncoding = QuantifiedHeaderValues.Select(q => q.HeaderValue).ToArray();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<string> ResponseContentEncoding { get; }

        #endregion Public Properties

        #region Public Methods

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }

        #endregion Public Methods
    }
}