using System.IO;
using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Headers.Response;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.Http {

    internal abstract class CompressContentEncoder : IContentEncoder {

        #region Public Properties

        public abstract ContentEncodingHeader ContentEncodingHeader { get; }

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods

        #region Protected Methods

        protected abstract Stream GetCompressStream(MemoryStream memoryStream);

        #endregion Protected Methods
    }
}