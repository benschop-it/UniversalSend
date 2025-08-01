using System.Collections.Generic;
using System.Collections.Immutable;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas {

    internal class GetResponse : RestResponse, IGetResponse {

        #region Public Enums

        public enum ResponseStatus : int {
            OK = 200,
            NotFound = 404
        };

        #endregion Public Enums

        #region Public Constructors

        public GetResponse(ResponseStatus status, IReadOnlyDictionary<string, string> headers, object data) : base((int)status, headers) {
            Status = status;
            ContentData = data;
        }

        public GetResponse(ResponseStatus status) : this(status, ImmutableDictionary<string, string>.Empty, null) {
        }

        public GetResponse(ResponseStatus status, IReadOnlyDictionary<string, string> headers) : this(status, headers, null) {
        }

        public GetResponse(ResponseStatus status, object data) : this(status, ImmutableDictionary<string, string>.Empty, data) {
        }

        #endregion Public Constructors

        #region Public Properties

        public object ContentData { get; }
        public ResponseStatus Status { get; }

        #endregion Public Properties
    }
}