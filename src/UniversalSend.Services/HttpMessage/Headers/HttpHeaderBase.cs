using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers {

    internal abstract class HttpHeaderBase : IHttpHeader {

        #region Protected Constructors

        protected HttpHeaderBase(string name, string value) {
            Name = name;
            Value = value;
        }

        #endregion Protected Constructors

        #region Public Properties

        public string Name { get; }
        public string Value { get; }

        #endregion Public Properties
    }
}