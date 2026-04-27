using System;
using System.Threading.Tasks;

namespace UniversalSend.Services.Interfaces {

    public interface IServiceHttpServer {

        #region Public Methods

        Task<bool> StartHttpServerAsync(int port);

        string GetBrowserDownloadUrl();

        void StopHttpServer();

        #endregion Public Methods

    }
}