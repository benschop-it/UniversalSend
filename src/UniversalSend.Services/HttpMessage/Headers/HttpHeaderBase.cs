using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers {

    public abstract class HttpHeaderBase : IHttpHeader {
        public string Name { get; }
        public string Value { get; }

        protected HttpHeaderBase(string name, string value) {
            Name = name;
            Value = value;
        }
    }
}