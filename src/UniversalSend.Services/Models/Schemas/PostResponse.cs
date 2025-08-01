using System.Collections.Generic;
using System.Collections.Immutable;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas {

    internal class PostResponse : RestResponse, IPostResponse {

        #region Public Enums

        public enum ResponseStatus : int {
            OK = 200,
            Created = 201,
            Conflict = 409
        };

        #endregion Public Enums

        #region Public Constructors

        public PostResponse(ResponseStatus status, string locationRedirectUri, object content, IReadOnlyDictionary<string, string> headers) : base((int)status, headers) {
            Status = status;
            LocationRedirect = locationRedirectUri;
            ContentData = content;
        }

        public PostResponse(ResponseStatus status, string locationRedirectUri, object content) : this(status, locationRedirectUri, content, ImmutableDictionary<string, string>.Empty) {
        }

        public PostResponse(ResponseStatus status, string locationRedirectUri, IReadOnlyDictionary<string, string> headers) : this(status, locationRedirectUri, null, headers) {
        }

        public PostResponse(ResponseStatus status, string locationRedirectUri) : this(status, locationRedirectUri, null, ImmutableDictionary<string, string>.Empty) {
        }

        public PostResponse(ResponseStatus status) : this(status, null, null, ImmutableDictionary<string, string>.Empty) {
        }

        #endregion Public Constructors

        #region Public Properties

        public object ContentData { get; }
        public string LocationRedirect { get; }
        public ResponseStatus Status { get; }

        #endregion Public Properties
    }
}