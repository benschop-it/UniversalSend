using System.Collections.Generic;
using System.Text;

namespace UniversalSend.Services.Helpers {

    internal class StringHelper {

        public static string ByteArrayToString(byte[] bytes) {
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }

        public static string GetURLFromURLWithQueryParmeters(string url) {
            int queryIndex = url.IndexOf('?');
            if (queryIndex == -1) {
                return url;
            }

            string baseUrl = url.Substring(0, queryIndex);
            string queryString = url.Substring(queryIndex + 1);
            string[] parameters = queryString.Split('&');

            string newQuery = "";
            for (int i = 0; i < parameters.Length; i++) {
                int equalsIndex = parameters[i].IndexOf('=');
                if (equalsIndex != -1) {
                    string paramName = parameters[i].Substring(0, equalsIndex);
                    newQuery += (i > 0 ? "&" : "") + paramName + "={}";
                }
            }

            return baseUrl + "?" + newQuery;
        }

        public static Dictionary<string, string> GetURLQueryParameters(string url) {
            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>();
            int queryIndex = url.IndexOf('?');
            if (queryIndex == -1) {
                return parameterDictionary;
            }

            string queryString = url.Substring(queryIndex + 1);
            string[] parameterPairs = queryString.Split('&');

            foreach (string pair in parameterPairs) {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2) {
                    string key = keyValue[0];
                    string value = keyValue[1];
                    parameterDictionary[key] = value;
                }
            }

            return parameterDictionary;
        }
    }
}