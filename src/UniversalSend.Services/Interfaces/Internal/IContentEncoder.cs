using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Headers.Response;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IContentEncoder {
        ContentEncodingHeader ContentEncodingHeader { get; }

        Task<byte[]> Encode(byte[] content);
    }
}