using Restup.Webserver.Http;
using Restup.Webserver.Rest;
using Restup.WebServer;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UniversalSend.Services {

    public class ServiceHttpServer {

        #region Private Fields

        private bool _isRunning = false;

        #endregion Private Fields

        #region Public Properties

        public HttpServer HttpServer { get; set; }

        public HttpServerConfiguration HttpServerConfiguration { get; set; }

        #endregion Public Properties

        #region Public Methods

        public async Task<bool> StartHttpServerAsync(int port) {
            RestRouteHandler restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<V1RequestController>(); // Register controller
            //restRouteHandler.RegisterController<V2RequestController>(); // Register controller
            HttpServerConfiguration = new HttpServerConfiguration();
            HttpServerConfiguration.ListenOnPort(port).RegisterRoute("api/localsend/", restRouteHandler).EnableCors();

            HttpServer = new HttpServer(HttpServerConfiguration);
            try {
                await HttpServer.StartServerAsync();
            } catch {
                return false;
            }

            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v1/send?fileId={}&token={}")) {
                OperationController.UriOperations.Add(
                    "/api/localsend/v1/send?fileId={}&token={}",
                    OperationFunctions.SendRequestFuncAsync
                );
            }

            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v1/register")) {
                OperationController.UriOperations.Add(
                    "/api/localsend/v1/register",
                    OperationFunctions.RegisterRequestFunc
                );
            }

            Debug.WriteLine($"HTTP server started on port {port}");
            return _isRunning = true;
        }

        public void StopHttpServer() {
            HttpServer.StopServer();
            _isRunning = false;
            Debug.WriteLine($"HTTP server has stopped");
        }

        #endregion Public Methods
    }
}