using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using Windows.UI.Xaml.Data;

namespace UniversalSend.Models
{
    public class StringHelper
    {
        public static string GetURLFromURLWithQueryParmeters(string url)
        {
            int queryIndex = url.IndexOf('?');
            if (queryIndex == -1)
            {
                return url;
            }

            string baseUrl = url.Substring(0, queryIndex);
            string queryString = url.Substring(queryIndex + 1);
            string[] parameters = queryString.Split('&');

            string newQuery = "";
            for (int i = 0; i < parameters.Length; i++)
            {
                int equalsIndex = parameters[i].IndexOf('=');
                if (equalsIndex != -1)
                {
                    string paramName = parameters[i].Substring(0, equalsIndex);
                    newQuery += (i > 0 ? "&" : "") + paramName + "={}";
                }
            }

            return baseUrl + "?" + newQuery;
        }

        public static Dictionary<string,string> GetURLQueryParameters(string url)
        {
            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>();
            int queryIndex = url.IndexOf('?');
            if (queryIndex == -1)
            {
                return parameterDictionary;
            }

            string queryString = url.Substring(queryIndex + 1);
            string[] parameterPairs = queryString.Split('&');

            foreach (string pair in parameterPairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0];
                    string value = keyValue[1];
                    parameterDictionary[key] = value;
                }
            }

            return parameterDictionary;
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }

        public static bool IsIpaddr(string str)
        {
            System.Net.IPAddress ipAddress;
            if (System.Net.IPAddress.TryParse(str, out ipAddress))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static readonly string[] Units = { "B", "KB", "MB", "GB"};
        private static readonly double[] UnitSizes = {
            1,
            1024,
            1024 * 1024,
            1024 * 1024 * 1024,
        };

        public static string GetByteUnit(long byteNum)
        {
            int unitIndex = 0;
            double value = byteNum;

            // 确定最适合的单位
            while (value >= 1024 && unitIndex < Units.Length - 1)
            {
                value /= 1024;
                unitIndex++;
            }


            if (unitIndex > 3)
                unitIndex = 3;
            return Units[unitIndex];
        }
    }

    public class HistoryStringConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is History)
            {
                History history = (History)value;
                string deviceAlias = "未知设备";
                if(history.Device!=null)
                {
                    deviceAlias = history.Device.Alias;
                }
                return $"{history.DateTime.ToString()} - {history.File.Size}{StringHelper.GetByteUnit(history.File.Size)} - {deviceAlias}";
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
