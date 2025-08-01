using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using Windows.Storage.Streams;

namespace UniversalSend.Services.HttpMessage.Plumbing {

    /// <summary>
    /// Wrapper around an <see cref="IInputStream"/> which supports a retry meganism
    /// if the stream is empty.
    /// </summary>
    /// <remarks>
    /// This wrapper doesn't handle disposing the stream, because it's not
    /// the owner of the stream.
    /// </remarks>
    internal class HttpRequestStream {

        #region Private Fields

        private static readonly int[] INPUTSTREAM_EMPTY_DELAYMS = new int[] { 1, 10, 10 };

        private IInputStream _requestStream;

        #endregion Private Fields

        #region Public Constructors

        public HttpRequestStream(IInputStream requestStream) {
            _requestStream = requestStream;
        }

        #endregion Public Constructors

        #region Internal Methods

        internal async Task<StreamReadResult> ReadAsync(uint bufferSize, InputStreamOptions streamOptions) {
            IBuffer response = null;
            int retryCounter = 0;
            do {
                var buffer = (new byte[bufferSize]).AsBuffer();
                response = await _requestStream.ReadAsync(buffer, bufferSize, streamOptions);

                if (response.Length == 0) {
                    await Task.Delay(INPUTSTREAM_EMPTY_DELAYMS[retryCounter]);
                    retryCounter++;
                }
            } while (response.Length == 0 && retryCounter < INPUTSTREAM_EMPTY_DELAYMS.Length);

            bool succeed = response.Length > 0;
            byte[] data = succeed ? response.ToArray() : new byte[0];

            return new StreamReadResult(data, succeed);
        }

        #endregion Internal Methods
    }
}