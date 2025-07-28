using System.Collections.Generic;
using UniversalSend.Services.HttpMessage.Models.Schemas;

namespace UniversalSend.Services.Models.Schemas
{
    internal class MethodNotAllowedResponse : StatusOnlyResponse
    {
        public IEnumerable<HttpMethod> Allows { get; }

        internal MethodNotAllowedResponse(IEnumerable<HttpMethod> allows) : base(405)
        {
            Allows = allows;
        }
    }
}
