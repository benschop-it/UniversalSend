using System.Collections.Generic;

namespace UniversalSend.Services.Rest.Models.Contracts {

    internal interface IRestResponse {
        int StatusCode { get; }
        IReadOnlyDictionary<string, string> Headers { get; }
    }
}