using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Http;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.Interfaces.Internal;
using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services.Rest {

    internal class RestControllerMethodWithContentExecutor : RestMethodExecutor {

        #region Private Fields

        private readonly ContentSerializer _contentSerializer;
        private readonly RestResponseFactory _responseFactory;
        private readonly IInstanceCreatorCache _instanceCreatorCache;

        #endregion Private Fields

        #region Public Constructors

        public RestControllerMethodWithContentExecutor(IInstanceCreatorCache instanceCreatorCache) {
            _instanceCreatorCache = instanceCreatorCache ?? throw new ArgumentNullException(nameof(instanceCreatorCache));
            _contentSerializer = new ContentSerializer();
            _responseFactory = new RestResponseFactory();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request, ParsedUri requestUri) {
            var instantiator = _instanceCreatorCache.GetCreator(info.MethodInfo.DeclaringType);

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

            //TODO: Not the best place to do this, but we are losing the headers further down the line...
            if (parameters.Length == 1) {
                if (parameters[0] is ISendRequestDataV2 sendRequestData) {
                    IEnumerable<IHttpRequestHeader> headers = request.HttpServerRequest.Headers;
                    foreach (var header in headers) {
                        if (header.Name == "host") {
                            sendRequestData.Host = header.Value;
                            break;
                        }
                    }
                }
            }
            return info.MethodInfo.Invoke(
                    instantiator.Create(info.MethodInfo.DeclaringType, info.ControllerConstructorArgs()),
                    parameters);
        }

        #endregion Protected Methods
    }
}