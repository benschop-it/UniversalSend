using System;
using System.Linq;
using UniversalSend.Services.InstanceCreators;
using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services.Rest {

    internal class RestControllerMethodExecutor : RestMethodExecutor {

        #region Private Fields

        private readonly RestResponseFactory _responseFactory;

        #endregion Private Fields

        #region Public Constructors

        public RestControllerMethodExecutor() {
            _responseFactory = new RestResponseFactory();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request, ParsedUri requestUri) {
            var instantiator = InstanceCreatorCache.Default.GetCreator(info.MethodInfo.DeclaringType);

            object[] parameters;
            try {
                parameters = info.GetParametersFromUri(requestUri).ToArray();
            } catch (FormatException) {
                return _responseFactory.CreateBadRequest();
            }

            return info.MethodInfo.Invoke(
                    instantiator.Create(info.MethodInfo.DeclaringType, info.ControllerConstructorArgs()),
                    parameters);
        }

        #endregion Protected Methods
    }
}