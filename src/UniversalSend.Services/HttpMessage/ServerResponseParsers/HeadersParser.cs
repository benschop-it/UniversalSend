using System.Text;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Plumbing;

namespace UniversalSend.Services.HttpMessage.ServerResponseParsers
{
    internal class HeadersParser : IHttpResponsePartParser
    {
        public byte[] ParseToBytes(HttpServerResponse response)
        {
            return Constants.DefaultHttpEncoding.GetBytes(ParseToString(response));
        }

        public string ParseToString(HttpServerResponse response)
        {
            var headersTextBuilder = new StringBuilder();
            foreach (var header in response.Headers)
            {
                headersTextBuilder.Append($"{header.Name}: {header.Value}\r\n");
            }

            headersTextBuilder.Append("\r\n");

            return headersTextBuilder.ToString();
        }
    }
}
