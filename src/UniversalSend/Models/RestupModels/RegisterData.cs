using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Models.RestupModels
{
    public sealed class RegisterData
    {
        public string alias { get; set; }
        public string version { get; set; }
        public string deviceModel { get; set; }
        public string deviceType { get; set; }
        public string fingerprint { get; set; }
        public int port { get; set; }
        public string protocol { get; set; }
        public bool download { get; set; }
        public bool announce { get; set; }

    }
}
