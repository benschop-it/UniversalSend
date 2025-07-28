using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace UniversalSend.Models {

    public class JsonHelper {

        #region Public Methods

        public static Dictionary<string, string> JObjectToDictionary(JObject jObject) {
            return jObject.ToObject<Dictionary<string, string>>();
        }

        #endregion Public Methods
    }
}