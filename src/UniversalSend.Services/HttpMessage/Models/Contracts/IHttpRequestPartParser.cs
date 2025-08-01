namespace UniversalSend.Services.HttpMessage.Models.Contracts {

    internal interface IHttpRequestPartParser {

        #region Public Properties

        bool IsFinished { get; }

        bool IsSucceeded { get; }

        byte[] UnparsedData { get; }

        #endregion Public Properties

        #region Public Methods

        void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar);

        #endregion Public Methods
    }
}