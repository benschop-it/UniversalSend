using System;
using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.Interfaces.Internal;
using UniversalSend.Services.Misc;
using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.Http {

    internal class RouteRegistration : IComparable<RouteRegistration> {

        #region Private Fields

        private readonly IRouteHandler _routeHandler;
        private readonly string _urlPrefix;

        #endregion Private Fields

        #region Public Constructors

        public RouteRegistration(string urlPrefix, IRouteHandler routeHandler) {
            _urlPrefix = urlPrefix.FormatRelativeUri();
            _routeHandler = routeHandler;
        }

        #endregion Public Constructors

        #region Public Methods

        public int CompareTo(RouteRegistration other) {
            return string.Compare(other._urlPrefix, _urlPrefix, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RouteRegistration)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return _urlPrefix?.GetHashCode() ?? 0;
            }
        }

        public async Task<HttpServerResponse> HandleAsync(IHttpServerRequest request) {
            var unPrefixedRequest = CreateHttpRequestWithUnprefixedUrl(request, _urlPrefix);

            return await _routeHandler.HandleRequest(unPrefixedRequest);
        }

        public bool Match(IHttpServerRequest request) {
            return request.Uri.ToString().StartsWith(_urlPrefix, StringComparison.OrdinalIgnoreCase);
        }

        #endregion Public Methods

        #region Protected Methods

        protected bool Equals(RouteRegistration other) {
            return string.Equals(_urlPrefix, other._urlPrefix);
        }

        #endregion Protected Methods

        #region Private Methods

        private static IHttpServerRequest CreateHttpRequestWithUnprefixedUrl(IHttpServerRequest request, string prefix) {
            var requestUriWithoutPrefix = request.Uri.RemovePrefix(prefix);

            return new HttpServerRequest(
                request.Headers,
                request.Method,
                requestUriWithoutPrefix,
                request.HttpVersion,
                request.ContentTypeCharset,
                request.AcceptCharsets,
                request.ContentLength,
                request.ContentType,
                request.AcceptEncodings,
                request.AcceptMediaTypes,
                request.Content,
                request.IsComplete,
                request.AccessControlRequestMethod,
                request.AccessControlRequestHeaders,
                request.Origin);
        }

        #endregion Private Methods
    }
}