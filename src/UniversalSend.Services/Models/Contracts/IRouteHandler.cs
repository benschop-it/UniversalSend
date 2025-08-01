using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.Models.Contracts {

    internal interface IRouteHandler {

        #region Public Methods

        Task<HttpServerResponse> HandleRequest(IHttpServerRequest request);

        #endregion Public Methods
    }
}