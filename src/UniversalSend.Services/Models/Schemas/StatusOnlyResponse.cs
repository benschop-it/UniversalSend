using System.Collections.Generic;
using System.Collections.Immutable;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas {

    internal class StatusOnlyResponse : RestResponse, IRestResponse {

        #region Internal Constructors

        internal StatusOnlyResponse(int statusCode, IReadOnlyDictionary<string, string> headers) : base(statusCode, headers) {
        }

        internal StatusOnlyResponse(int statusCode) : this(statusCode, ImmutableDictionary<string, string>.Empty) {
        }

        #endregion Internal Constructors
    }
}