using UniversalSend.Services.HttpMessage.Plumbing;

namespace UniversalSend.Services.HttpMessage.ServerRequestParsers {

    internal class ProtocolVersionParser : HttpRequestPartParser {

        #region Public Methods

        public override void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar) {
            var word = stream.ReadNextWord();

            if (word.WordFound) {
                resultThisFar.HttpVersion = word.Word;
                UnparsedData = word.RemainingBytes;
                IsFinished = true;
                IsSucceeded = true;
            }
        }

        #endregion Public Methods
    }
}