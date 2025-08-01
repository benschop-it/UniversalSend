using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IHttpServerResponseParser {

        #region Public Methods

        byte[] ConvertToBytes(HttpServerResponse response);

        string ConvertToString(HttpServerResponse response);

        #endregion Public Methods
    }
}