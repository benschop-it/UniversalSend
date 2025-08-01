using System;

namespace UniversalSend.Models.Interfaces {

    public interface ILogFactory : IDisposable {

        #region Public Methods

        ILogger GetLogger<T>();

        ILogger GetLogger(string name);

        #endregion Public Methods
    }
}