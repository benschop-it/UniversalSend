using UniversalSend.Services.HttpMessage.Models.Contracts;

namespace UniversalSend.Services.HttpMessage.ServerRequestParsers {

    internal abstract class HttpRequestPartParser : IHttpRequestPartParser {

        #region Public Properties

        public bool IsFinished { get; protected set; }

        public bool IsSucceeded { get; protected set; }

        public byte[] UnparsedData { get; protected set; }

        #endregion Public Properties

        #region Public Methods

        public abstract void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar);

        #endregion Public Methods
    }
}