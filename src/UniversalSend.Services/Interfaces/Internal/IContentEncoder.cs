using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Headers.Response;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IContentEncoder {

        #region Public Properties

        ContentEncodingHeader ContentEncodingHeader { get; }

        #endregion Public Properties

        #region Public Methods

        Task<byte[]> Encode(byte[] content);

        #endregion Public Methods
    }
}