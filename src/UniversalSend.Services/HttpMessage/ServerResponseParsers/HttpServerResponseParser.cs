using System.Collections.Generic;
using System.Text;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.HttpMessage.ServerResponseParsers {

    internal class HttpServerResponseParser : IHttpServerResponseParser {

        private IEnumerable<IHttpResponsePartParser> _pipeline;

        public HttpServerResponseParser(IHttpCodesTranslator httpCodesTranslator) {
            _pipeline = new IHttpResponsePartParser[] {
                new StartLineParser(httpCodesTranslator),
                new HeadersParser(),
                new ContentParser()
            };
        }

        public string ConvertToString(HttpServerResponse response) {
            var responseBuilder = new StringBuilder();
            foreach (var pipelinePart in _pipeline) {
                responseBuilder.Append(pipelinePart.ParseToString(response));
            }

            return responseBuilder.ToString();
        }

        public byte[] ConvertToBytes(HttpServerResponse response) {
            var responseBytes = new List<byte>();
            foreach (var pipelinePart in _pipeline) {
                responseBytes.AddRange(pipelinePart.ParseToBytes(response));
            }

            return responseBytes.ToArray();
        }
    }
}