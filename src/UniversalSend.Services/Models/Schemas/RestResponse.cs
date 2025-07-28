using System.Collections.Generic;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Models.Schemas
{
    public class RestResponse : IRestResponse
    {
        public int StatusCode { get; }
        public IReadOnlyDictionary<string, string> Headers { get; }

        public RestResponse(int statusCode, IReadOnlyDictionary<string, string> headers)
        {
            StatusCode = statusCode;
            Headers = headers;
        }
    }
}