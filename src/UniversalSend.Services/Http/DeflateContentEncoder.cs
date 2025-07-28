using System.IO;
using System.IO.Compression;
using UniversalSend.Services.HttpMessage.Headers.Response;

namespace UniversalSend.Services.Http
{
    internal class DeflateContentEncoder : CompressContentEncoder
    {
        public override ContentEncodingHeader ContentEncodingHeader { get; } = new ContentEncodingHeader("deflate");

        protected override Stream GetCompressStream(MemoryStream memoryStream)
        {
            return new DeflateStream(memoryStream, CompressionLevel.Optimal);
        }
    }
}