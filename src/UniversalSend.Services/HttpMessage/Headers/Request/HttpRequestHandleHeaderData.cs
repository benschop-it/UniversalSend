using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    /// <summary>
    /// Set specific properties on the <see cref="MutableHttpServerRequest"/> object based
    /// on the httpheader.
    /// </summary>
    /// <remarks>
    /// All methods in this class are thread safe
    /// </remarks>
    internal class HttpRequestHandleHeaderData : IHttpRequestHeaderVisitor<MutableHttpServerRequest> {

        #region Public Constructors

        static HttpRequestHandleHeaderData() {
            Default = new HttpRequestHandleHeaderData();
        }

        #endregion Public Constructors

        #region Private Constructors

        private HttpRequestHandleHeaderData() {
        }

        #endregion Private Constructors

        #region Internal Properties

        internal static HttpRequestHandleHeaderData Default { get; }

        #endregion Internal Properties

        #region Public Methods

        public void Visit(AcceptHeader uh, MutableHttpServerRequest arg) {
            arg.AcceptMediaTypes = uh.AcceptTypes;
        }

        public void Visit(AcceptCharsetHeader uh, MutableHttpServerRequest arg) {
            arg.AcceptCharsets = uh.ResponseContentEncoding;
        }

        public void Visit(AcceptEncodingHeader uh, MutableHttpServerRequest arg) {
            arg.AcceptEncodings = uh.AcceptEncodings;
        }

        public void Visit(AccessControlRequestMethodHeader uh, MutableHttpServerRequest arg) {
            arg.AccessControlRequestMethod = uh.Method;
        }

        public void Visit(AccessControlRequestHeadersHeader uh, MutableHttpServerRequest arg) {
            arg.AccessControlRequestHeaders = uh.Headers;
        }

        public void Visit(ContentTypeHeader uh, MutableHttpServerRequest arg) {
            arg.ContentTypeCharset = uh.ContentCharset;
            arg.ContentType = uh.ContentType;
        }

        public void Visit(ContentLengthHeader uh, MutableHttpServerRequest arg) {
            arg.ContentLength = uh.Length;
        }

        public void Visit(OriginHeader uh, MutableHttpServerRequest arg) {
            // no specific info to set for untyped header
            arg.Origin = uh.Value;
        }

        public void Visit(UntypedRequestHeader uh, MutableHttpServerRequest arg) {
            // no specific info to set for untyped header
        }

        #endregion Public Methods
    }
}