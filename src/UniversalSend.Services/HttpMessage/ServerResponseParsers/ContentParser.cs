using System.Text;
using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.ServerResponseParsers {

    internal class ContentParser : IHttpResponsePartParser {

        #region Private Fields

        private static Encoding DEFAULT_CONTENT_ENCODING = Encoding.UTF8;

        #endregion Private Fields

        #region Public Methods

        public byte[] ParseToBytes(HttpServerResponse response) {
            return response.Content ?? new byte[0];
        }

        public string ParseToString(HttpServerResponse response) {
            if (response.Content == null)
                return string.Empty;

            return DEFAULT_CONTENT_ENCODING.GetString(response.Content);
        }

        #endregion Public Methods
    }
}