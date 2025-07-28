using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Headers.Response;

namespace UniversalSend.Services.Http
{
    internal interface IContentEncoder
    {
        ContentEncodingHeader ContentEncodingHeader { get; }
        Task<byte[]> Encode(byte[] content);
    }
}