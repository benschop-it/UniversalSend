using System.Collections.Generic;
using System.Collections.Immutable;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas {

    internal class PutResponse : RestResponse, IPutResponse {

        #region Public Enums

        public enum ResponseStatus : int {
            OK = 200,
            NoContent = 204,
            NotFound = 404
        };

        #endregion Public Enums

        #region Public Constructors

        public PutResponse(ResponseStatus status, object content, IReadOnlyDictionary<string, string> headers) : base((int)status, headers) {
            Status = status;
            ContentData = content;
        }

        public PutResponse(ResponseStatus status, object content) : this(status, content, ImmutableDictionary<string, string>.Empty) {
        }

        public PutResponse(ResponseStatus status, IReadOnlyDictionary<string, string> headers) : this(status, null, headers) {
        }

        public PutResponse(ResponseStatus status) : this(status, null, ImmutableDictionary<string, string>.Empty) {
        }

        #endregion Public Constructors

        #region Public Properties

        public object ContentData { get; set; }
        public ResponseStatus Status { get; }

        #endregion Public Properties

        #region Public Methods

        public static PutResponse CreateNotFound() {
            return new PutResponse(ResponseStatus.NotFound, null);
        }

        #endregion Public Methods
    }
}