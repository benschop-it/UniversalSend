using System.Collections.Generic;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    internal class FileResponseDataManager : IFileResponseDataManager {

        #region Public Methods

        public FileResponseDataV2 CreateFromDictionary(Dictionary<string, string> keyValuePairs) {
            FileResponseDataV2 fileResponseData = new FileResponseDataV2();
            foreach (var item in keyValuePairs) {
                fileResponseData.Files.Add(item.Key, item.Value);
            }
            return fileResponseData;
        }

        #endregion Public Methods

    }
}