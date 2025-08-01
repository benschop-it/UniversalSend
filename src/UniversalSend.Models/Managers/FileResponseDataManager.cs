using System.Collections.Generic;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    internal class FileResponseDataManager : IFileResponseDataManager {

        #region Public Methods

        public FileResponseData CreateFromDictionary(Dictionary<string, string> keyValuePairs) {
            FileResponseData fileResponseData = new FileResponseData();
            foreach (var item in keyValuePairs) {
                fileResponseData.Add(item.Key, item.Value);
            }
            return fileResponseData;
        }

        #endregion Public Methods

    }
}