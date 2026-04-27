using System.Collections.Generic;
using System.Collections.Immutable;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas {

    internal class BinaryGetResponse : RestResponse, IGetResponse {

        public BinaryGetResponse(GetResponse.ResponseStatus status, byte[] content, IReadOnlyDictionary<string, string> headers = null)
            : base((int)status, headers ?? ImmutableDictionary<string, string>.Empty) {
            ContentData = content;
            Status = status;
        }

        public object ContentData { get; }

        public GetResponse.ResponseStatus Status { get; }
    }
}
