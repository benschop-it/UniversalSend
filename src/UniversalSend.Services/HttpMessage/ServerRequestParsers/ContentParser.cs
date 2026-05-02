using System;

namespace UniversalSend.Services.HttpMessage.ServerRequestParsers {

    /// <summary>
    /// One bad request scenario we don't cover:
    ///
    /// If the content has exactly the correct length, but somehow the stream is not done yet, it will stop processing
    /// and be marked as succesfull. Even though new data is being written to the stream, which would make it a bad
    /// request. The reason we don't cover this scenario is that the alternative is even worse, always wait for some timeout
    /// on the stream to be sure it's not writing unexpected data.
    /// </summary>
    internal class ContentParser : HttpRequestPartParser {

        #region Private Fields

        private byte[] _content;
        private int _offset;

        #endregion Private Fields

        #region Public Constructors

        public ContentParser() {
            // Incoming data is read entirely, always, so there will never be any unparsed data
            UnparsedData = new byte[0];
        }

        #endregion Public Constructors

        #region Public Methods

        public override void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar) {
            if (resultThisFar.ContentLength == 0) {
                IsFinished = true;
                IsSucceeded = true;
                resultThisFar.Content = Array.Empty<byte>();
            } else {
                EnsureContentBuffer(resultThisFar.ContentLength);

                int remaining = resultThisFar.ContentLength - _offset;
                int bytesToCopy = Math.Min(stream.Length, remaining);
                Array.Copy(stream, 0, _content, _offset, bytesToCopy);
                _offset += bytesToCopy;

                if (_offset == resultThisFar.ContentLength) {
                    resultThisFar.Content = _content;
                    IsFinished = true;
                    IsSucceeded = true;
                }
                // else if content is bigger, finished will never be set, badrequest will happen
            }
        }

        private void EnsureContentBuffer(int contentLength) {
            if (_content != null && _content.Length == contentLength) {
                return;
            }

            _content = new byte[contentLength];
            _offset = 0;
        }

        #endregion Public Methods
    }
}