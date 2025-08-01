using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Headers.Response;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.Http {

    internal class NoContentEncoder : IContentEncoder {

        #region Public Properties

        public ContentEncodingHeader ContentEncodingHeader { get; } = null;

        #endregion Public Properties

        #region Public Methods

        public Task<byte[]> Encode(byte[] content) {
            return Task.FromResult(content);
        }

        #endregion Public Methods
    }
}