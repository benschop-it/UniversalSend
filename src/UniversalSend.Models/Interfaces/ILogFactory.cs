using System;

namespace UniversalSend.Models.Interfaces {

    public interface ILogFactory : IDisposable {

        ILogger GetLogger<T>();

        ILogger GetLogger(string name);
    }
}