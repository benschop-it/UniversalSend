using System.Collections.Generic;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces {
    public interface IFileResponseDataManager {

        #region Public Methods

        FileResponseDataV2 CreateFromDictionary(Dictionary<string, string> keyValuePairs);

        #endregion Public Methods
    }
}