using System;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Plumbing;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.HttpMessage.ServerResponseParsers {

    internal class StartLineParser : IHttpResponsePartParser {
        private IHttpCodesTranslator _httpCodesTranslator;

        public StartLineParser(IHttpCodesTranslator httpCodesTranslator) {
            _httpCodesTranslator = httpCodesTranslator ?? throw new ArgumentNullException(nameof(httpCodesTranslator));
        }

        public byte[] ParseToBytes(HttpServerResponse response) {
            return Constants.DefaultHttpEncoding.GetBytes(ParseToString(response));
        }

        public string ParseToString(HttpServerResponse response) {
            var version = GetHttpVersion(response.HttpVersion);
            var status = (int)response.ResponseStatus;
            var statusText = _httpCodesTranslator.GetHttpStatusCodeText(status);

            return $"{version} {status} {statusText}\r\n";
        }

        private static string GetHttpVersion(Version httpVersion) {
            return $"HTTP/{httpVersion.Major}.{httpVersion.Minor}";
        }
    }
}