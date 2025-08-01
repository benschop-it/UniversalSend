using System;
using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.Interfaces.Internal;
using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.Rest {

    internal class RestRouteHandler : IRouteHandler {

        #region Private Fields

        private readonly RestControllerRequestHandler _requestHandler;
        private readonly RestServerRequestFactory _restServerRequestFactory;
        private readonly RestToHttpResponseConverter _restToHttpConverter;

        #endregion Private Fields

        #region Public Constructors

        public RestRouteHandler(IConfiguration configuration, IEncodingCache encodingCache) {
            _restServerRequestFactory = new RestServerRequestFactory(configuration, encodingCache);
            _requestHandler = new RestControllerRequestHandler();
            _restToHttpConverter = new RestToHttpResponseConverter();
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<HttpServerResponse> HandleRequest(IHttpServerRequest request) {
            var restServerRequest = _restServerRequestFactory.Create(request);

            var restResponse = await _requestHandler.HandleRequestAsync(restServerRequest);

            var httpResponse = _restToHttpConverter.ConvertToHttpResponse(restResponse, restServerRequest);

            return httpResponse;
        }

        public void RegisterController<T>() where T : class {
            _requestHandler.RegisterController<T>();
        }

        public void RegisterController<T>(params object[] args) where T : class {
            _requestHandler.RegisterController<T>(() => args);
        }

        public void RegisterController<T>(Func<object[]> args) where T : class {
            _requestHandler.RegisterController<T>(args);
        }

        #endregion Public Methods
    }
}