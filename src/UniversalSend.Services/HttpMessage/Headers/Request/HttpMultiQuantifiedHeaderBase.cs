using System.Collections.Generic;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal abstract class HttpMultiQuantifiedHeaderBase : HttpRequestHeaderBase {

        #region Protected Constructors

        protected HttpMultiQuantifiedHeaderBase(
            string name,
            string value,
            IEnumerable<QuantifiedHeaderValue> quantifiedHeaderValues) : base(name, value) {
            QuantifiedHeaderValues = quantifiedHeaderValues;
        }

        #endregion Protected Constructors

        #region Public Properties

        public IEnumerable<QuantifiedHeaderValue> QuantifiedHeaderValues { get; }

        #endregion Public Properties
    }
}