using System.IO;
using System.IO.Compression;
using UniversalSend.Services.HttpMessage.Headers.Response;

namespace UniversalSend.Services.Http {

    internal class GzipContentEncoder : CompressContentEncoder {
        public override ContentEncodingHeader ContentEncodingHeader { get; } = new ContentEncodingHeader("gzip");

        protected override Stream GetCompressStream(MemoryStream memoryStream) {
            return new GZipStream(memoryStream, CompressionLevel.Optimal);
        }
    }
}