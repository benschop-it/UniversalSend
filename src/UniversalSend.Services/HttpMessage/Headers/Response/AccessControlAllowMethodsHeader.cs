using System.Collections.Generic;
using UniversalSend.Services.HttpMessage.Models.Schemas;

namespace UniversalSend.Services.HttpMessage.Headers.Response
{
    public class AccessControlAllowMethodsHeader : HttpHeaderBase
    {
        internal static string NAME = "Access-Control-Allow-Methods";

        public AccessControlAllowMethodsHeader(IEnumerable<HttpMethod> methods) : base(NAME, string.Join(", ", methods))
        {
        }
    }
}