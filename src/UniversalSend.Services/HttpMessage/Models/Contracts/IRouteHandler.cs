using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Models.Contracts
{
    public interface IRouteHandler
    {
        Task<HttpServerResponse> HandleRequest(IHttpServerRequest request);
    }
}