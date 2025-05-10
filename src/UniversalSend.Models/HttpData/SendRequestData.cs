using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Models.HttpData
{
    public sealed class SendRequestData
    {
        public InfoData info { get; set; }
        public Dictionary<string, FileRequestData>files { get;set; }
    }
}
