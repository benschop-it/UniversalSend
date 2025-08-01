using System.Collections.Generic;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces {
    public interface IFileResponseDataManager {
        FileResponseData CreateFromDictionary(Dictionary<string, string> keyValuePairs);
    }
}