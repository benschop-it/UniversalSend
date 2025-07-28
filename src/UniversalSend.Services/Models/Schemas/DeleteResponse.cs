using System.Collections.Generic;
using System.Collections.Immutable;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas {

    public class DeleteResponse : RestResponse, IDeleteResponse {

        public enum ResponseStatus {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        public ResponseStatus Status { get; }

        public DeleteResponse(ResponseStatus status, IReadOnlyDictionary<string, string> headers) : base((int)status, headers) {
            Status = status;
        }

        public DeleteResponse(ResponseStatus status) : this(status, ImmutableDictionary<string, string>.Empty) {
        }
    }
}