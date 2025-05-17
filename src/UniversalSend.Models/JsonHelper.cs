using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Models
{
    public class JsonHelper
    {
        public static Dictionary<string, string> JObjectToDictionary(JObject jObject)
        {
            return jObject.ToObject<Dictionary<string, string>>();
        }
    }
}
