namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class ContentEncodingHeader : HttpHeaderBase {
        internal static string NAME = "Content-Encoding";

        public string Encoding { get; }

        public ContentEncodingHeader(string encoding) : base(NAME, encoding) {
            Encoding = encoding;
        }
    }
}