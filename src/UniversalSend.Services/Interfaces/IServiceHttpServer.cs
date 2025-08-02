using System;
using System.Threading.Tasks;

namespace UniversalSend.Services.Interfaces {

    public interface IServiceHttpServer {

        #region Public Events

        event EventHandler<HttpParseProgressEventArgs> HttpRequestProgressChanged;

        #endregion Public Events

        #region Public Methods

        Task<bool> StartHttpServerAsync(int port);

        void StopHttpServer();

        #endregion Public Methods

    }
}