using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Interfaces {
    public interface IOperationFunctions {
        object RegisterRequestFunc(IMutableHttpServerRequest mutableHttpServerRequest);
        Task<object> SendRequestFuncAsync(IMutableHttpServerRequest mutableHttpServerRequest);
    }
}