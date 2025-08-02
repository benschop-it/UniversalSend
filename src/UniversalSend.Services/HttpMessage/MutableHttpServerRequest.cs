using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Headers.Request;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.HttpMessage.ServerRequestParsers;
using UniversalSend.Services.Interfaces.Internal;
using Windows.Storage.Streams;

namespace UniversalSend.Services.HttpMessage {

    internal class MutableHttpServerRequest : IHttpServerRequest, IMutableHttpServerRequest {

        #region Private Fields

        private readonly List<IHttpRequestHeader> _headers;
        private readonly HttpRequestParser _httpRequestParser;

        #endregion Private Fields

        #region Internal Constructors

        internal MutableHttpServerRequest(HttpRequestParser httpRequestParser) {
            _headers = new List<IHttpRequestHeader>();
            _httpRequestParser = httpRequestParser;

            AcceptCharsets = Enumerable.Empty<string>();
            AcceptMediaTypes = Enumerable.Empty<string>();
            AcceptEncodings = Enumerable.Empty<string>();
            AccessControlRequestHeaders = Enumerable.Empty<string>();
        }

        #endregion Internal Constructors

        #region Public Properties

        public IEnumerable<string> AcceptCharsets { get; set; }
        public IEnumerable<string> AcceptEncodings { get; set; }
        public IEnumerable<string> AcceptMediaTypes { get; set; }
        public IEnumerable<string> AccessControlRequestHeaders { get; set; }
        public HttpMethod? AccessControlRequestMethod { get; set; }
        public byte[] Content { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string ContentTypeCharset { get; set; }
        public IEnumerable<IHttpRequestHeader> Headers => _headers;
        public string HttpVersion { get; set; }
        public bool IsComplete { get; set; }
        public HttpMethod? Method { get; set; }
        public string Origin { get; set; }
        public Uri Uri { get; set; }

        #endregion Public Properties

        #region Internal Methods

        internal void AddHeader(IHttpRequestHeader header) {
            if (IsComplete) {
                throw new InvalidOperationException("Can't add header after processing request is finished!");
            }

            header.Visit(HttpRequestHandleHeaderData.Default, this);

            _headers.Add(header);
        }

        #endregion Internal Methods
    }
}