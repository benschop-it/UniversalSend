using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSend.Models.Interfaces {
    public interface IServiceHttpServer {
        Task<bool> StartHttpServerAsync(int port);
        void StopHttpServer();
    }
}
