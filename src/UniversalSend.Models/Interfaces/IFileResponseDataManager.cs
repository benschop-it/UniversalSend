using System.Collections.Generic;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces {
    public interface IFileResponseDataManager {

        #region Public Methods

        FileResponseData CreateFromDictionary(Dictionary<string, string> keyValuePairs);

        #endregion Public Methods
    }
}