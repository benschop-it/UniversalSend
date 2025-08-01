using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Services {
    public interface IEncodingCache {
        Encoding GetEncoding(string charset);
    }
}
