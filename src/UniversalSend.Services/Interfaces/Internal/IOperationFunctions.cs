using System.Threading.Tasks;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IOperationFunctions {

        #region Public Methods

        object RegisterRequestFunc(IMutableHttpServerRequest mutableHttpServerRequest);

        Task<object> SendRequestFuncAsync(IMutableHttpServerRequest mutableHttpServerRequest);

        #endregion Public Methods
    }
}