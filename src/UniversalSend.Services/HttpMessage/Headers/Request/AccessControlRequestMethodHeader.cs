using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.HttpMessage.ServerRequestParsers;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class AccessControlRequestMethodHeader : HttpRequestHeaderBase {

        #region Internal Fields

        internal static string NAME = "Access-Control-Request-Method";

        #endregion Internal Fields

        #region Public Constructors

        public AccessControlRequestMethodHeader(string value) : base(NAME, value) {
            Method = HttpMethodParser.GetMethod(value);
        }

        #endregion Public Constructors

        #region Public Properties

        public HttpMethod Method { get; }

        #endregion Public Properties

        #region Public Methods

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }

        #endregion Public Methods
    }
}