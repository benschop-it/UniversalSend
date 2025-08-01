using System;
using System.Collections.Generic;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.HttpMessage {

    internal class HttpServerRequest : IHttpServerRequest {

        #region Public Constructors

        public HttpServerRequest(IEnumerable<IHttpRequestHeader> headers, HttpMethod? method, Uri uri,
            string httpVersion, string contentTypeCharset, IEnumerable<string> acceptCharsets, int contentLength,
            string contentType, IEnumerable<string> acceptEncodings, IEnumerable<string> acceptMediaTypes,
            byte[] content, bool isComplete, HttpMethod? accessControlRequestMethod, IEnumerable<string> accessControlRequestHeaders,
            string origin) {
            Headers = headers;
            Method = method;
            Uri = uri;
            HttpVersion = httpVersion;
            ContentTypeCharset = contentTypeCharset;
            AcceptCharsets = acceptCharsets;
            ContentLength = contentLength;
            ContentType = contentType;
            AcceptEncodings = acceptEncodings;
            AcceptMediaTypes = acceptMediaTypes;
            Content = content;
            IsComplete = isComplete;
            AccessControlRequestMethod = accessControlRequestMethod;
            AccessControlRequestHeaders = accessControlRequestHeaders;
            Origin = origin;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<string> AcceptCharsets { get; }
        public IEnumerable<string> AcceptEncodings { get; }
        public IEnumerable<string> AcceptMediaTypes { get; }
        public IEnumerable<string> AccessControlRequestHeaders { get; }
        public HttpMethod? AccessControlRequestMethod { get; }
        public byte[] Content { get; }
        public int ContentLength { get; }
        public string ContentType { get; }
        public string ContentTypeCharset { get; }
        public IEnumerable<IHttpRequestHeader> Headers { get; }
        public string HttpVersion { get; }
        public bool IsComplete { get; }
        public HttpMethod? Method { get; }
        public string Origin { get; }
        public Uri Uri { get; }

        #endregion Public Properties
    }
}