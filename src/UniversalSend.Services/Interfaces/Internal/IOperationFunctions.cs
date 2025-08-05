using System.Threading.Tasks;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IOperationFunctions {

        #region Public Methods

        object RegisterRequestFuncV2(IMutableHttpServerRequest mutableHttpServerRequest);

        Task<object> SendRequestFuncAsync(IMutableHttpServerRequest mutableHttpServerRequest);

        Task<object> UploadRequestFuncAsync(IMutableHttpServerRequest mutableHttpServerRequest);

        #endregion Public Methods
    }
}