using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.Tasks
{
    public class SendTask
    {
        public UniversalSendFile file { get; set; }
        public byte[] fileContent { get; set; }
    }

    public class SendTaskManager
    {
        //public List<>
    }
}
