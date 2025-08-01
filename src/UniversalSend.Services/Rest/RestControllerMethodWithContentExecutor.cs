using Newtonsoft.Json;
using System;
using System.Linq;
using UniversalSend.Services.Http;
using UniversalSend.Services.InstanceCreators;
using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services.Rest {

    internal class RestControllerMethodWithContentExecutor : RestMethodExecutor {

        #region Private Fields

        private readonly ContentSerializer _contentSerializer;
        private readonly RestResponseFactory _responseFactory;

        #endregion Private Fields

        #region Public Constructors

        public RestControllerMethodWithContentExecutor() {
            _contentSerializer = new ContentSerializer();
            _responseFactory = new RestResponseFactory();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request, ParsedUri requestUri) {
            var instantiator = InstanceCreatorCache.Default.GetCreator(info.MethodInfo.DeclaringType);

            object contentObj = null;
            try {
                if (request.HttpServerRequest.Content != null) {
                    contentObj = _contentSerializer.FromContent(
                        request.ContentEncoding.GetString(request.HttpServerRequest.Content),
                        request.ContentMediaType,
                        info.ContentParameterType);
                }
            } catch (JsonReaderException) {
                return _responseFactory.CreateBadRequest();
            } catch (InvalidOperationException) {
                return _responseFactory.CreateBadRequest();
            }

            object[] parameters = null;
            try {
                parameters = info.GetParametersFromUri(requestUri).Concat(new[] { contentObj }).ToArray();
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