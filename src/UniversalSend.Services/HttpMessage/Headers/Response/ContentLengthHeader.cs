namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class ContentLengthHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Content-Length";

        #endregion Internal Fields

        #region Public Constructors

        public ContentLengthHeader(int length) : base(NAME, length.ToString()) {
            Length = length;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Length { get; }

        #endregion Public Properties
    }
}