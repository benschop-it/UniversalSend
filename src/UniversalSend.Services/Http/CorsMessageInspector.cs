using System;
using System.Collections.Generic;
using System.Linq;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.HttpMessage.Headers.Response;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.Http {

    internal class CorsMessageInspector : IHttpMessageInspector {

        #region Private Fields

        private readonly bool _allOriginsAllowed = false;
        private readonly IEnumerable<string> _allowedOrigins;

        #endregion Private Fields

        #region Public Constructors

        public CorsMessageInspector(IEnumerable<string> allowedOrigins) {
            _allowedOrigins = allowedOrigins ?? Enumerable.Empty<string>();
            if (_allowedOrigins.Contains("*"))
                _allOriginsAllowed = true;
        }

        #endregion Public Constructors

        #region Public Methods

        public AfterHandleRequestResult AfterHandleRequest(IHttpServerRequest request, HttpServerResponse httpResponse) {
            string origin;
            if (!TryGetAllowOrigin(request.Origin, out origin))
                return null;

            httpResponse.AddHeader(new AccessControlAllowOriginHeader(origin));
            return new AfterHandleRequestResult(httpResponse);
        }

        public BeforeHandleRequestResult BeforeHandleRequest(IHttpServerRequest request) {
            // could potentially pass this as state between the before handle request and after handle request
            // but before that would need to see if the performance increase is worth it
            string allowOrigin;
            if (request.Method != HttpMethod.OPTIONS || !TryGetAllowOrigin(request.Origin, out allowOrigin))
                return null;

            var httpResponse = HttpServerResponse.Create(HttpResponseStatus.OK);
            httpResponse.AddHeader(new AccessControlAllowMethodsHeader(new[] { HttpMethod.GET, HttpMethod.POST, HttpMethod.PUT, HttpMethod.DELETE, HttpMethod.OPTIONS, }));
            // max age possible by chrome https://code.google.com/p/chromium/codesearch#chromium/src/third_party/WebKit/Source/core/loader/CrossOriginPreflightResultCache.cpp&l=40&rcl=1399481969
            httpResponse.AddHeader(new AccessControlMaxAgeHeader(10 * 60));
            if (request.AccessControlRequestHeaders.Any())
                httpResponse.AddHeader(new AccessControlAllowHeadersHeader(request.AccessControlRequestHeaders));

            return new BeforeHandleRequestResult(httpResponse);
        }

        #endregion Public Methods

        #region Private Methods

        private bool TryGetAllowOrigin(string requestOrigin, out string allowedOrigin) {
            if (string.IsNullOrWhiteSpace(requestOrigin)) {
                allowedOrigin = null;
                return false;
            }

            if (_allOriginsAllowed) {
                allowedOrigin = "*";
                return true;
            }

            allowedOrigin = _allowedOrigins.FirstOrDefault(x => string.Equals(requestOrigin, x, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(allowedOrigin))
                return true;

            return false;
        }

        #endregion Private Methods
    }
}