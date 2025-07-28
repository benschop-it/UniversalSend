using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Headers.Response;

namespace UniversalSend.Services.Http {

    internal class NoContentEncoder : IContentEncoder {
        public ContentEncodingHeader ContentEncodingHeader { get; } = null;

        public Task<byte[]> Encode(byte[] content) {
            return Task.FromResult(content);
        }
    }
}