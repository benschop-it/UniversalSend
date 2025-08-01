using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Headers.Response;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.Http {

    internal class NoContentEncoder : IContentEncoder {
        public ContentEncodingHeader ContentEncodingHeader { get; } = null;

        public Task<byte[]> Encode(byte[] content) {
            return Task.FromResult(content);
        }
    }
}