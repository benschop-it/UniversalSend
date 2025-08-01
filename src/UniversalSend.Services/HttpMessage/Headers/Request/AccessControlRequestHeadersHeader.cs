using System.Collections.Generic;
using System.Linq;
using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class AccessControlRequestHeadersHeader : HttpRequestHeaderBase {

        #region Internal Fields

        internal static string NAME = "Access-Control-Request-Headers";

        #endregion Internal Fields

        #region Public Constructors

        public AccessControlRequestHeadersHeader(string value) : base(NAME, value) {
            Headers = Parse(value);
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<string> Headers { get; }

        #endregion Public Properties

        #region Public Methods

        public override void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg) {
            v.Visit(this, arg);
        }

        #endregion Public Methods

        #region Private Methods

        private static IEnumerable<string> Parse(string value) {
            return value.Split(',').Select(x => x.Trim());
        }

        #endregion Private Methods
    }
}