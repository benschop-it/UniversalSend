using System.Collections.Generic;
using System.Linq;
using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class AcceptEncodingHeader : HttpMultiQuantifiedHeaderBase {

        #region Internal Fields

        internal static string NAME = "Accept-Encoding";

        #endregion Internal Fields

        #region Public Constructors

        public AcceptEncodingHeader(string value, IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues)
            : base(NAME, value, quantifiedHeaderValues) {
            AcceptEncodings = QuantifiedHeaderValues.Select(GetAcceptEncoding).ToArray();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<string> AcceptEncodings { get; }

        #endregion Public Properties

        #region Public Methods

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetAcceptEncoding(QuantifiedHeaderValue arg) {
            return arg.HeaderValue;
        }

        #endregion Private Methods
    }
}