using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Models.HttpData
{
    public sealed class FileResponseData : Dictionary<string, string> //fileId,token
    {
    }

    public class FileResponseDataManager
    {
        public static FileResponseData CreateFromDictionary(Dictionary<string, string>keyValuePairs)
        {
            FileResponseData fileResponseData = new FileResponseData();
            foreach(var item in keyValuePairs)
            {
                fileResponseData.Add(item.Key,item.Value);
            }
            return fileResponseData;
        }
    }
}
