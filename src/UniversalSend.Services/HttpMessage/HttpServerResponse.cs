﻿using System;
using System.Collections.Generic;
using System.Linq;
using UniversalSend.Services.HttpMessage.Headers;
using UniversalSend.Services.HttpMessage.Headers.Response;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.HttpMessage.Plumbing;
using UniversalSend.Services.HttpMessage.ServerResponseParsers;

namespace UniversalSend.Services.HttpMessage {

    internal class HttpServerResponse {

        #region Private Fields

        private static HttpCodesTranslator _httpCodesTranslator = new HttpCodesTranslator();
        private static HttpServerResponseParser _httpServerResponseParser;
        private readonly List<IHttpHeader> _headers;

        #endregion Private Fields

        #region Private Constructors

        private HttpServerResponse(Version httpVersion, HttpResponseStatus status) {
            if (_httpServerResponseParser == null) {
                _httpServerResponseParser = new HttpServerResponseParser(_httpCodesTranslator);
            }
            _headers = new List<IHttpHeader>();

            HttpVersion = httpVersion;
            ResponseStatus = status;
        }

        #endregion Private Constructors

        #region Public Properties

        public IEnumerable<HttpMethod> Allow {
            get {
                var allowHeader = Headers.OfType<AllowHeader>().SingleOrDefault();

                return allowHeader?.Allows ?? Enumerable.Empty<HttpMethod>();
            }
            set {
                var allowHeader = Headers.OfType<AllowHeader>().SingleOrDefault();
                _headers.Remove(allowHeader);

                if (value != null) {
                    _headers.Add(new AllowHeader(value));
                }
            }
        }

        // Content
        public byte[] Content { get; set; }

        public string ContentCharset {
            get {
                return Headers.OfType<ContentTypeHeader>().SingleOrDefault()?.Charset;
            }
            set {
                var contentTypeHeader = Headers.OfType<ContentTypeHeader>().SingleOrDefault();
                if (contentTypeHeader == null) {
                    contentTypeHeader = new ContentTypeHeader(string.Empty, value);
                    _headers.Add(contentTypeHeader);
                } else {
                    _headers.Remove(contentTypeHeader);
                    _headers.Add(new ContentTypeHeader(contentTypeHeader.ContentType, value));
                }
            }
        }

        public string ContentType {
            get {
                return Headers.OfType<ContentTypeHeader>().SingleOrDefault()?.ContentType;
            }
            set {
                var contentTypeHeader = Headers.OfType<ContentTypeHeader>().SingleOrDefault();
                if (value == null && contentTypeHeader != null) {
                    _headers.Remove(contentTypeHeader);
                } else if (!string.IsNullOrWhiteSpace(value) && contentTypeHeader == null) {
                    contentTypeHeader = new ContentTypeHeader(value, null);
                    _headers.Add(contentTypeHeader);
                } else if (!string.IsNullOrWhiteSpace(value)) {
                    _headers.Remove(contentTypeHeader);
                    _headers.Add(new ContentTypeHeader(value, contentTypeHeader.Charset));
                }
            }
        }

        public DateTime? Date {
            get {
                return Headers.OfType<DateHeader>().SingleOrDefault()?.Date;
            }
            set {
                var dateHeader = Headers.OfType<DateHeader>().SingleOrDefault();
                _headers.Remove(dateHeader);

                if (value.HasValue) {
                    _headers.Add(new DateHeader(value.Value));
                }
            }
        }

        // Header line info
        public Version HttpVersion { get; set; }

        public bool IsConnectionClosed {
            get {
                return Headers.OfType<CloseConnectionHeader>().Any();
            }
            set {
                var closeConnHeader = Headers.OfType<CloseConnectionHeader>().SingleOrDefault();
                if (value && closeConnHeader == null) {
                    _headers.Add(new CloseConnectionHeader());
                } else if (!value && closeConnHeader != null) {
                    _headers.Remove(closeConnHeader);
                }
            }
        }

        public Uri Location {
            get {
                return Headers.OfType<LocationHeader>().SingleOrDefault()?.Location;
            }
            set {
                var locationHeader = Headers.OfType<LocationHeader>().SingleOrDefault();
                _headers.Remove(locationHeader);

                if (value != null) {
                    _headers.Add(new LocationHeader(value));
                }
            }
        }

        public HttpResponseStatus ResponseStatus { get; set; }

        #endregion Public Properties

        #region Internal Properties

        internal IEnumerable<IHttpHeader> Headers => _headers;

        #endregion Internal Properties

        #region Public Methods

        public static HttpServerResponse Create(int statusCode) {
            return Create((HttpResponseStatus)statusCode);
        }

        public static HttpServerResponse Create(HttpResponseStatus status) {
            return Create(new Version(1, 1), status);
        }

        public static HttpServerResponse Create(Version httpVersion, HttpResponseStatus status) {
            return new HttpServerResponse(httpVersion, status);
        }
        public IHttpHeader AddHeader(string name, string value) {
            var knownHeader = Headers.SingleOrDefault(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase));
            _headers.Remove(knownHeader);

            var newHeader = new UntypedResponseHeader(name, value);
            _headers.Add(newHeader);

            return newHeader;
        }

        /// <summary>
        /// Will update header if a header with the same name already exists.
        /// </summary>
        public void AddHeader(IHttpHeader header) {
            var knownHeader = Headers.SingleOrDefault(h => string.Equals(h.Name, header.Name, StringComparison.OrdinalIgnoreCase));
            _headers.Remove(knownHeader);
            _headers.Add(header);
        }

        public void RemoveHeader(string name) {
            var knownHeader = Headers.SingleOrDefault(h => string.Equals(h.Name, name, StringComparison.OrdinalIgnoreCase));
            _headers.Remove(knownHeader);
        }

        public void RemoveHeader(IHttpHeader header) {
            _headers.Remove(header);
        }

        public byte[] ToBytes() {
            return _httpServerResponseParser.ConvertToBytes(this);
        }

        public override string ToString() {
#if DEBUG
            // This is just used for debugging purposes and will not be available when running in release mode. Problem with
            // this method is that it uses Encoding to decode the content which is a fairly complicated process. For debugging
            // purposes I'm using UTF-8 which is working most of the time. In real life you want to use the charset provided, or
            // some default encoding as explained in the HTTP specs.
            return _httpServerResponseParser.ConvertToString(this);
#else
            return $"{HttpVersion} {ResponseStatus} including {Headers.Count()} headers.";
#endif
        }

        #endregion Public Methods
    }
}