using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Interfaces.Internal {
    internal interface IOperationFunctions {
        object RegisterRequestFunc(IMutableHttpServerRequest mutableHttpServerRequest);
        Task<object> SendRequestFuncAsync(IMutableHttpServerRequest mutableHttpServerRequest);
    }
}