using System;
using System.Linq;
using UniversalSend.Services.Interfaces.Internal;
using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services.Rest {

    internal class RestServerRequestFactory {

        #region Private Fields

        private readonly IConfiguration _configuration;
        private readonly IEncodingCache _encodingCache;

        #endregion Private Fields

        #region Public Constructors

        public RestServerRequestFactory(IConfiguration configuration, IEncodingCache encodingCache) {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _encodingCache = encodingCache ?? throw new ArgumentNullException(nameof(encodingCache));
        }

        #endregion Public Constructors

        #region Public Methods

        public RestServerRequest Create(IHttpServerRequest httpRequest) {
            var acceptMediaType = GetAcceptMediaType(httpRequest);

            var acceptCharset = GetAcceptCharset(httpRequest, acceptMediaType);

            var contentMediaType = GetContentMediaType(httpRequest);

            var contentCharset = GetContentCharset(httpRequest, contentMediaType);

            return new RestServerRequest(
                httpRequest,
                acceptCharset,
                acceptMediaType,
                _encodingCache.GetEncoding(acceptCharset),
                contentMediaType,
                contentCharset,
                _encodingCache.GetEncoding(contentCharset)
            );
        }

        #endregion Public Methods

        #region Private Methods

        private static MediaType GetMediaType(string contentType) {
            if ("application/json".Equals(contentType, StringComparison.OrdinalIgnoreCase) ||
                "text/json".Equals(contentType, StringComparison.OrdinalIgnoreCase))
                return MediaType.JSON;

            if ("application/xml".Equals(contentType, StringComparison.OrdinalIgnoreCase) ||
                "text/xml".Equals(contentType, StringComparison.OrdinalIgnoreCase))
                return MediaType.XML;

            return MediaType.Unsupported;
        }

        private string GetAcceptCharset(IHttpServerRequest httpRequest, MediaType acceptMediaType) {
            string firstAvailableEncoding = null;

            foreach (var requestedCharset in httpRequest.AcceptCharsets) {
                var encoding = _encodingCache.GetEncoding(requestedCharset);
                firstAvailableEncoding = requestedCharset;

                if (encoding != null)
                    break;
            }

            if (string.IsNullOrEmpty(firstAvailableEncoding)) {
                if (acceptMediaType == MediaType.JSON) {
                    firstAvailableEncoding = _configuration.DefaultJSONCharset;
                } else if (acceptMediaType == MediaType.XML) {
                    firstAvailableEncoding = _configuration.DefaultXMLCharset;
                } else {
                    throw new NotImplementedException("Accept media type is not supported.");
                }
            }

            return firstAvailableEncoding;
        }

        private MediaType GetAcceptMediaType(IHttpServerRequest httpRequest) {
            var preferredType = httpRequest.AcceptMediaTypes
                                    .Select(GetMediaType)
                                    .FirstOrDefault(a => a != MediaType.Unsupported);

            if (preferredType == MediaType.Unsupported)
                preferredType = _configuration.DefaultAcceptType;

            return preferredType;
        }

        private string GetContentCharset(IHttpServerRequest httpRequest, MediaType contentMediaType) {
            var requestContentCharset = httpRequest.ContentTypeCharset;
            var encoding = _encodingCache.GetEncoding(requestContentCharset);
            if (encoding == null) {
                if (contentMediaType == MediaType.JSON) {
                    requestContentCharset = _configuration.DefaultJSONCharset;
                } else if (contentMediaType == MediaType.XML) {
                    requestContentCharset = _configuration.DefaultXMLCharset;
                } else {
                    throw new NotImplementedException("Content media type is not supported.");
                }
            }

            return requestContentCharset;
        }

        private MediaType GetContentMediaType(IHttpServerRequest httpRequest) {
            var contentMediaType = GetMediaType(httpRequest.ContentType ?? string.Empty); // guard against nulls
            if (contentMediaType == MediaType.Unsupported)
                return _configuration.DefaultContentType;

            return contentMediaType;
        }

        #endregion Private Methods
    }
}