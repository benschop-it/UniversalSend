namespace UniversalSend.Services.HttpMessage.Models.Contracts {

    internal interface IHttpResponsePartParser {

        #region Public Methods

        byte[] ParseToBytes(HttpServerResponse response);

        string ParseToString(HttpServerResponse response);

        #endregion Public Methods
    }
}