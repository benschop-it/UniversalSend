using UniversalSend.Services.HttpMessage.Plumbing;

namespace UniversalSend.Services.HttpMessage.ServerRequestParsers {

    internal class MethodParser : HttpRequestPartParser {

        public override void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar) {
            var word = stream.ReadNextWord();

            if (word.WordFound) {
                resultThisFar.Method = HttpMethodParser.GetMethod(word.Word);
                UnparsedData = word.RemainingBytes;
                IsFinished = true;
                IsSucceeded = true;
            }
        }
    }
}