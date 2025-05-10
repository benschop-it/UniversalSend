using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Models.HttpData
{
    public class FileRequestData
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; } // in bytes
        public string FileType { get; set; }
        //public string Preview { get; set; } // Nullable
    }
}
