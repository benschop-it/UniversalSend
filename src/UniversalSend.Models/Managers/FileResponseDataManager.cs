using System.Collections.Generic;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Managers {
    internal class FileResponseDataManager {

        #region Public Methods

        public static FileResponseData CreateFromDictionary(Dictionary<string, string> keyValuePairs) {
            FileResponseData fileResponseData = new FileResponseData();
            foreach (var item in keyValuePairs) {
                fileResponseData.Add(item.Key, item.Value);
            }
            return fileResponseData;
        }

        #endregion Public Methods
    }
}