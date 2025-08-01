﻿using System;
using UniversalSend.Services.HttpMessage.Plumbing;

namespace UniversalSend.Services.HttpMessage.ServerRequestParsers {

    internal class ResourceIdentifierParser : HttpRequestPartParser {

        #region Public Methods

        public override void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar) {
            var word = stream.ReadNextWord();

            if (word.WordFound) {
                resultThisFar.Uri = new Uri(word.Word, UriKind.RelativeOrAbsolute);
                UnparsedData = word.RemainingBytes;
                IsFinished = true;
                IsSucceeded = true;
            }
        }

        #endregion Public Methods
    }
}