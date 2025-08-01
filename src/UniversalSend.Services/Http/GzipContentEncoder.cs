using System.IO;
using System.IO.Compression;
using UniversalSend.Services.HttpMessage.Headers.Response;

namespace UniversalSend.Services.Http {

    internal class GzipContentEncoder : CompressContentEncoder {

        #region Public Properties

        public override ContentEncodingHeader ContentEncodingHeader { get; } = new ContentEncodingHeader("gzip");

        #endregion Public Properties

        #region Protected Methods

        protected override Stream GetCompressStream(MemoryStream memoryStream) {
            return new GZipStream(memoryStream, CompressionLevel.Optimal);
        }

        #endregion Protected Methods
    }
}