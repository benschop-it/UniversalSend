using System;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.Plumbing;

namespace UniversalSend.Services.ServerRequestParsers
{
    internal class ResourceIdentifierParser : HttpRequestPartParser
    {
        public override void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar)
        {
            var word = stream.ReadNextWord();

            if (word.WordFound)
            {
                resultThisFar.Uri = new Uri(word.Word, UriKind.RelativeOrAbsolute);
                UnparsedData = word.RemainingBytes;
                IsFinished = true;
                IsSucceeded = true;
            }
        }
    }
}
