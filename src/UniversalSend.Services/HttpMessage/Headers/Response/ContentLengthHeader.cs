namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class ContentLengthHeader : HttpHeaderBase {
        internal static string NAME = "Content-Length";

        public int Length { get; }

        public ContentLengthHeader(int length) : base(NAME, length.ToString()) {
            Length = length;
        }
    }
}