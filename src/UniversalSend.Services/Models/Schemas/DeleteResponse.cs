using System.Collections.Generic;
using System.Collections.Immutable;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas {

    internal class DeleteResponse : RestResponse, IDeleteResponse {

        #region Public Enums

        public enum ResponseStatus {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        #endregion Public Enums

        #region Public Constructors

        public DeleteResponse(ResponseStatus status, IReadOnlyDictionary<string, string> headers) : base((int)status, headers) {
            Status = status;
        }

        public DeleteResponse(ResponseStatus status) : this(status, ImmutableDictionary<string, string>.Empty) {
        }

        #endregion Public Constructors

        #region Public Properties

        public ResponseStatus Status { get; }

        #endregion Public Properties
    }
}