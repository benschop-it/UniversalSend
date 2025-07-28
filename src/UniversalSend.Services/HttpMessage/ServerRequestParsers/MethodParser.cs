using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.HttpMessage.ServerRequestParsers;
using UniversalSend.Services.Plumbing;

namespace UniversalSend.Services.ServerRequestParsers
{
    internal class MethodParser : HttpRequestPartParser
    {
        public override void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar)
        {
            var word = stream.ReadNextWord();

            if (word.WordFound)
            {
                resultThisFar.Method = HttpMethodParser.GetMethod(word.Word);
                UnparsedData = word.RemainingBytes;
                IsFinished = true;
                IsSucceeded = true;
            }
        }
    }
}
