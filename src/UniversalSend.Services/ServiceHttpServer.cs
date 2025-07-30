using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Controllers;
using UniversalSend.Services.Http;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.Rest;

namespace UniversalSend.Services {

    internal class ServiceHttpServer : IServiceHttpServer {

        private readonly IRegisterResponseDataManager _registerResponseDataManager;
        private readonly IDeviceManager _deviceManager;
        private readonly IRegister _register;
        private readonly ISettings _settings;
        private OperationFunctions _operationFunctions;

        public ServiceHttpServer(
            IRegisterResponseDataManager registerResponseDataManager,
            IDeviceManager deviceManager,
            IRegister register,
            ISettings settings,
            OperationFunctions operationFunctions
        ) {
            _registerResponseDataManager = registerResponseDataManager ?? throw new ArgumentNullException(nameof(registerResponseDataManager));
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
            _register = register ?? throw new ArgumentNullException(nameof(register));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _operationFunctions = operationFunctions ?? throw new ArgumentNullException(nameof(operationFunctions));    
        }

        #region Private Fields

        private bool _isRunning = false;
        private UdpDiscoveryService _udpDiscovery;

        #endregion Private Fields

        #region Public Properties

        internal HttpServer HttpServer { get; set; }

        internal HttpServerConfiguration HttpServerConfiguration { get; set; }

        #endregion Public Properties

        #region Public Methods

        public async Task<bool> StartHttpServerAsync(int port) {
            _udpDiscovery = new UdpDiscoveryService(_registerResponseDataManager, _deviceManager, _register, _settings);
            await _udpDiscovery.StartUdpListenerAsync();

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
                    _operationFunctions.SendRequestFuncAsync
                );
            }

            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v1/register")) {
                OperationController.UriOperations.Add(
                    "/api/localsend/v1/register",
                    _operationFunctions.RegisterRequestFunc
                );
            }

            Debug.WriteLine($"HTTP server started on port {port}");
            return _isRunning = true;
        }

        public void StopHttpServer() {
            _udpDiscovery?.StopUdpListener();
            _udpDiscovery = null;

            HttpServer.StopServer();
            _isRunning = false;
            Debug.WriteLine($"HTTP server has stopped");
        }

        #endregion Public Methods

    }
}