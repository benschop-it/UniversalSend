using System;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Plumbing;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.HttpMessage.ServerResponseParsers {

    internal class StartLineParser : IHttpResponsePartParser {

        #region Private Fields

        private IHttpCodesTranslator _httpCodesTranslator;

        #endregion Private Fields

        #region Public Constructors

        public StartLineParser(IHttpCodesTranslator httpCodesTranslator) {
            _httpCodesTranslator = httpCodesTranslator ?? throw new ArgumentNullException(nameof(httpCodesTranslator));
        }

        #endregion Public Constructors

        #region Public Methods

        public byte[] ParseToBytes(HttpServerResponse response) {
            return Constants.DefaultHttpEncoding.GetBytes(ParseToString(response));
        }

        public string ParseToString(HttpServerResponse response) {
            var version = GetHttpVersion(response.HttpVersion);
            var status = (int)response.ResponseStatus;
            var statusText = _httpCodesTranslator.GetHttpStatusCodeText(status);

            return $"{version} {status} {statusText}\r\n";
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetHttpVersion(Version httpVersion) {
            return $"HTTP/{httpVersion.Major}.{httpVersion.Minor}";
        }

        #endregion Private Methods
    }
}