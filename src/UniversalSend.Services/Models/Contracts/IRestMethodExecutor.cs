using System.Threading.Tasks;
using UniversalSend.Services.Models.Schemas;
using UniversalSend.Services.Rest;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Contracts
{
    interface IRestMethodExecutor
    {
        Task<IRestResponse> ExecuteMethodAsync(RestControllerMethodInfo info, RestServerRequest request, ParsedUri requestUri);
    }
}
