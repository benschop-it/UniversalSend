namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal abstract class HttpSingleQuantifiedHeaderBase : HttpRequestHeaderBase {

        #region Protected Constructors

        protected HttpSingleQuantifiedHeaderBase(string name, string value, QuantifiedHeaderValue quantifiedHeaderValue)
            : base(name, value) {
            QuantifiedHeaderValue = quantifiedHeaderValue;
        }

        #endregion Protected Constructors

        #region Public Properties

        public QuantifiedHeaderValue QuantifiedHeaderValue { get; }

        #endregion Public Properties
    }
}