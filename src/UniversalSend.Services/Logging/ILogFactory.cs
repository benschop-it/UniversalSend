using System;

namespace UniversalSend.Services.Logging {

    public interface ILogFactory : IDisposable {

        ILogger GetLogger<T>();

        ILogger GetLogger(string name);
    }
}