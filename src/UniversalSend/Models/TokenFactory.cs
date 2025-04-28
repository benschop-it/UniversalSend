using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Models
{
    public class TokenFactory
    {
        public static string CreateToken()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}
