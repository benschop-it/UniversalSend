using System;
using System.Linq;

namespace UniversalSend.Services.Rest {

    internal class QueryParameterValueGetter : ParameterValueGetter {

        #region Private Fields

        private readonly string _queryParameterName;

        #endregion Private Fields

        #region Public Constructors

        public QueryParameterValueGetter(string methodName, Type parameterType, UriParameter uriParameter)
            : base(methodName, parameterType) {
            _queryParameterName = uriParameter.Name;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override string GetValueFromUri(ParsedUri parsedUri) {
            return parsedUri.Parameters.FirstOrDefault(x => x.Name.Equals(_queryParameterName, StringComparison.OrdinalIgnoreCase)).Value;
        }

        #endregion Protected Methods
    }
}