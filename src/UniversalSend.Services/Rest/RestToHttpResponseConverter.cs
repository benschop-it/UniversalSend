﻿using System;
using UniversalSend.Services.Http;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.Models.Schemas;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Rest {

    internal class RestToHttpResponseConverter {

        #region Private Fields

        private readonly ContentSerializer _contentSerializer;

        #endregion Private Fields

        #region Public Constructors

        public RestToHttpResponseConverter() {
            _contentSerializer = new ContentSerializer();
        }

        #endregion Public Constructors

        #region Internal Methods

        internal HttpServerResponse ConvertToHttpResponse(IRestResponse restResponse, RestServerRequest restServerRequest) {
            var methodNotAllowedResponse = restResponse as MethodNotAllowedResponse;
            if (methodNotAllowedResponse != null)
                return GetMethodNotAllowedResponse(methodNotAllowedResponse);

            var postResponse = restResponse as PostResponse;
            if (postResponse != null)
                return GetPostResponse(postResponse, restServerRequest);

            var response = restResponse as IContentRestResponse;
            if (response != null)
                return GetDefaultContentResponse(response, restServerRequest);

            return GetDefaultResponse(restResponse);
        }

        #endregion Internal Methods

        #region Private Methods

        private static HttpServerResponse GetDefaultResponse(IRestResponse response) {
            var serverResponse = HttpServerResponse.Create(response.StatusCode);
            serverResponse.Date = DateTime.Now;
            serverResponse.IsConnectionClosed = true;
            foreach (var header in response.Headers) {
                serverResponse.AddHeader(header.Key, header.Value);
            }

            return serverResponse;
        }

        private static HttpServerResponse GetMethodNotAllowedResponse(MethodNotAllowedResponse methodNotAllowedResponse) {
            var serverResponse = GetDefaultResponse(methodNotAllowedResponse);
            serverResponse.Allow = methodNotAllowedResponse.Allows;

            return serverResponse;
        }

        private HttpServerResponse GetDefaultContentResponse(IContentRestResponse response, RestServerRequest restReq) {
            var defaultResponse = GetDefaultResponse(response);

            if (response.ContentData != null) {
                defaultResponse.ContentType = GetMediaTypeAsString(restReq.AcceptMediaType);
                defaultResponse.ContentCharset = restReq.AcceptCharset;
                defaultResponse.Content = _contentSerializer.ToAcceptContent(response.ContentData, restReq);
            }

            return defaultResponse;
        }

        private string GetMediaTypeAsString(MediaType acceptMediaType) {
            switch (acceptMediaType) {
                case MediaType.JSON:
                    return "application/json";
                case MediaType.XML:
                    return "application/xml";
                case MediaType.Unsupported:
                    return "";
            }

            throw new ArgumentException($"Don't know how to convert {nameof(acceptMediaType)}.");
        }

        private HttpServerResponse GetPostResponse(PostResponse response, RestServerRequest restReq) {
            var serverResponse = GetDefaultContentResponse(response, restReq);

            var locationRedirect = response.LocationRedirect;
            if (response.Status == PostResponse.ResponseStatus.Created && !string.IsNullOrWhiteSpace(locationRedirect)) {
                serverResponse.Location = new Uri(locationRedirect, UriKind.RelativeOrAbsolute);
            }

            return serverResponse;
        }

        #endregion Private Methods
    }
}