using System.Collections.Generic;

namespace UniversalSend.Models.HttpData {

    /// <summary>
    /// Dictionary that maps FileId to Token
    /// </summary>
    public sealed class FileResponseData : Dictionary<string, string> {
    }

    public class FileResponseDataManager {

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