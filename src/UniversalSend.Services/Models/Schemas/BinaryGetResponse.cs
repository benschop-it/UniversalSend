using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas {

    internal class BinaryGetResponse : RestResponse, IGetResponse {

        public BinaryGetResponse(GetResponse.ResponseStatus status, byte[] content, string contentType, IReadOnlyDictionary<string, string> headers = null)
            : base((int)status, BuildHeaders(contentType, headers)) {
            ContentBytes = content;
            ContentData = content;
            Status = status;
        }

        public BinaryGetResponse(GetResponse.ResponseStatus status, Stream contentStream, long? contentLength, string contentType, IReadOnlyDictionary<string, string> headers = null)
            : base((int)status, BuildHeaders(contentType, headers)) {
            ContentStream = contentStream;
            StreamContentLength = contentLength;
            Status = status;
        }

        public byte[] ContentBytes { get; }

        public object ContentData { get; }

        public Stream ContentStream { get; }

        public GetResponse.ResponseStatus Status { get; }

        public long? StreamContentLength { get; }

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
