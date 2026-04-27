using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas {

    internal class BinaryGetResponse : RestResponse, IGetResponse {

        public BinaryGetResponse(GetResponse.ResponseStatus status, byte[] content, string contentType, IReadOnlyDictionary<string, string> headers = null)
            : base((int)status, BuildHeaders(contentType, headers)) {
            ContentData = content;
            Status = status;
        }

        public object ContentData { get; }

        public GetResponse.ResponseStatus Status { get; }

        private static IReadOnlyDictionary<string, string> BuildHeaders(string contentType, IReadOnlyDictionary<string, string> headers) {
            var builder = (headers ?? ImmutableDictionary<string, string>.Empty)
                .ToDictionary(x => x.Key, x => x.Value);
            if (!string.IsNullOrWhiteSpace(contentType)) {
                builder["Content-Type"] = contentType;
            }

            return builder.ToImmutableDictionary();
        }
    }
}
