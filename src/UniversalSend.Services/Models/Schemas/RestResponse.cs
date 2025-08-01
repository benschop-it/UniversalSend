using System.Collections.Generic;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas {

    internal class RestResponse : IRestResponse {

        #region Public Constructors

        public RestResponse(int statusCode, IReadOnlyDictionary<string, string> headers) {
            StatusCode = statusCode;
            Headers = headers;
        }

        #endregion Public Constructors

        #region Public Properties

        public IReadOnlyDictionary<string, string> Headers { get; }
        public int StatusCode { get; }

        #endregion Public Properties
    }
}