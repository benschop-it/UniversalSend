using System.IO;
using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Headers.Response;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.Http {

    internal abstract class CompressContentEncoder : IContentEncoder {
        public abstract ContentEncodingHeader ContentEncodingHeader { get; }

        public async Task<byte[]> Encode(byte[] content) {
            if (content == null)
                return null;

            using (var memoryStream = new MemoryStream()) {
                using (var deflateStream = GetCompressStream(memoryStream)) {
                    await deflateStream.WriteAsync(content, 0, content.Length);
                }
                return memoryStream.ToArray();
            }
        }

        protected abstract Stream GetCompressStream(MemoryStream memoryStream);
    }
}