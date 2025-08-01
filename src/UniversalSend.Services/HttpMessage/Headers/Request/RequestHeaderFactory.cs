﻿using System;
using System.Collections.Generic;
using System.Linq;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Plumbing;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class RequestHeaderFactory {

        #region Private Fields

        private readonly Dictionary<string, Func<string, IHttpRequestHeader>> _headerCollection;

        #endregion Private Fields

        #region Internal Constructors

        internal RequestHeaderFactory() {
            _headerCollection = new Dictionary<string, Func<string, IHttpRequestHeader>>(StringComparer.OrdinalIgnoreCase) {
                [ContentLengthHeader.NAME] = x => new ContentLengthHeader(x),
                [AcceptHeader.NAME] = CreateResponseContentType,
                [ContentTypeHeader.NAME] = CreateRequestContentType,
                [AcceptCharsetHeader.NAME] = CreateResponseContentCharset,
                [AcceptEncodingHeader.NAME] = CreateResponseAcceptEncoding,
                [AccessControlRequestHeadersHeader.NAME] = x => new AccessControlRequestHeadersHeader(x),
                [AccessControlRequestMethodHeader.NAME] = x => new AccessControlRequestMethodHeader(x),
                [OriginHeader.NAME] = x => new OriginHeader(x)
            };
        }

        #endregion Internal Constructors

        #region Internal Methods

        internal IHttpRequestHeader Create(string headerName, string headerValue) {
            if (_headerCollection.ContainsKey(headerName)) {
                return _headerCollection[headerName](headerValue);
            }

            return new UntypedRequestHeader(headerName, headerValue);
        }

        internal QuantifiedHeaderValue ExtractQuantifiedHeader(string value) {
            string headerValue = null;
            var extractedQuantifiers = new Dictionary<string, string>();
            var quantifiers = value.Split(';');
            if (quantifiers.Length > 0) {
                headerValue = quantifiers[0].TrimWhitespaces();
            }
            if (quantifiers.Length > 1) {
                foreach (var quantifier in quantifiers.Skip(1)) {
                    var parts = quantifier.Split('=');
                    if (parts.Length > 1) {
                        string qKey = parts[0].TrimWhitespaces();
                        string qValue = parts[1].TrimWhitespaces();
                        extractedQuantifiers.Add(qKey, qValue);
                    }
                }
            }

            return new QuantifiedHeaderValue(headerValue, extractedQuantifiers);
        }

        internal IEnumerable<QuantifiedHeaderValue> ExtractQuantifiedHeaders(string value) {
            var headerValues = value.Split(',');
            var quantifiedValues = headerValues.Select(ExtractQuantifiedHeader);

            return quantifiedValues.OrderByDescending(q => q.Quality).ToArray();
        }

        #endregion Internal Methods

        #region Private Methods

        private IHttpRequestHeader CreateRequestContentType(string headerValue) {
            return new ContentTypeHeader(headerValue, ExtractQuantifiedHeader(headerValue));
        }

        private IHttpRequestHeader CreateResponseAcceptEncoding(string headerValue) {
            return new AcceptEncodingHeader(headerValue, ExtractQuantifiedHeaders(headerValue));
        }

        private IHttpRequestHeader CreateResponseContentCharset(string headerValue) {
            return new AcceptCharsetHeader(headerValue, ExtractQuantifiedHeaders(headerValue));
        }

        private IHttpRequestHeader CreateResponseContentType(string headerValue) {
            return new AcceptHeader(headerValue, ExtractQuantifiedHeaders(headerValue));
        }

        #endregion Private Methods
    }
}