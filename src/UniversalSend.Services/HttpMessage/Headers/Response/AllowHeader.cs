using System.Collections.Generic;
using UniversalSend.Services.HttpMessage.Models.Schemas;

namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class AllowHeader : HttpHeaderBase {
        internal static string NAME = "Allow";

        public IEnumerable<HttpMethod> Allows { get; }

        public AllowHeader(IEnumerable<HttpMethod> allows) : base(NAME, string.Join(";", allows)) {
            Allows = allows;
        }
    }
}