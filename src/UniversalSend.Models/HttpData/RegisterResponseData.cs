using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Models.HttpData
{
    public class RegisterResponseData
    {
        public string alias { get; set; }
        public string deviceModel { get; set; }
        public string deviceType { get; set; }
        public string fingerprint { get; set; }
        public bool announcement { get; set; }

    }
}
