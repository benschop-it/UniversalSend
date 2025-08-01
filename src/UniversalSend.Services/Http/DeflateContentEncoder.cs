using System.IO;
using System.IO.Compression;
using UniversalSend.Services.HttpMessage.Headers.Response;

namespace UniversalSend.Services.Http {

    internal class DeflateContentEncoder : CompressContentEncoder {

        #region Public Properties

        public override ContentEncodingHeader ContentEncodingHeader { get; } = new ContentEncodingHeader("deflate");

        #endregion Public Properties

        #region Protected Methods

        protected override Stream GetCompressStream(MemoryStream memoryStream) {
            return new DeflateStream(memoryStream, CompressionLevel.Optimal);
        }

        #endregion Protected Methods
    }
}