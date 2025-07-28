using System.Collections.Generic;

namespace UniversalSend.Services.Models.Contracts
{
    public interface IRestResponse
    {
        int StatusCode { get; }
        IReadOnlyDictionary<string, string> Headers { get; }
    }
}
