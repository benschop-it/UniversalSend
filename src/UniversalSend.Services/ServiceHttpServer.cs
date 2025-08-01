using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Controllers;
using UniversalSend.Services.Http;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.Interfaces;
using UniversalSend.Services.Interfaces.Internal;
using UniversalSend.Services.Rest;

namespace UniversalSend.Services {

    internal class ServiceHttpServer : IServiceHttpServer {

        private readonly IDeviceManager _deviceManager;
        private readonly IRegister _register;
        private readonly ISettings _settings;
        private readonly IOperationFunctions _operationFunctions;
        private IInfoDataManager _infoDataManager;
        private IReceiveManager _receiveManager;
        private ITokenFactory _tokenFactory;
        private IUniversalSendFileManager _universalSendFileManager;
        private IReceiveTaskManager _receiveTaskManager;
        private IRegisterResponseDataManager _registerResponseDataManager;
        private readonly IHttpRequestParser _httpRequestParser;
        private readonly IConfiguration _configuration;
        private readonly IEncodingCache _encodingCache;

        public ServiceHttpServer(
            IDeviceManager deviceManager,
            IRegister register,
            ISettings settings,
            IOperationFunctions operationFunctions,
            IInfoDataManager infoDataManager,
            IReceiveManager receiveManager,
            ITokenFactory tokenFactory,
            IUniversalSendFileManager universalSendFileManager,
            IReceiveTaskManager receiveTaskManager,
            IRegisterResponseDataManager registerResponseDataManager,
            IHttpRequestParser httpRequestParser,
            IConfiguration configuration,
            IEncodingCache encodingCache
        ) {
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
            _register = register ?? throw new ArgumentNullException(nameof(register));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _operationFunctions = operationFunctions ?? throw new ArgumentNullException(nameof(operationFunctions));
            _infoDataManager = infoDataManager ?? throw new ArgumentNullException(nameof(infoDataManager));
            _receiveManager = receiveManager ?? throw new ArgumentNullException(nameof(receiveManager));
            _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
            _universalSendFileManager = universalSendFileManager ?? throw new ArgumentNullException(nameof(universalSendFileManager));
            _receiveTaskManager = receiveTaskManager ?? throw new ArgumentNullException(nameof(receiveTaskManager));
            _registerResponseDataManager = registerResponseDataManager ?? throw new ArgumentNullException(nameof(registerResponseDataManager));
            _httpRequestParser = httpRequestParser ?? throw new ArgumentNullException(nameof(httpRequestParser));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _encodingCache = encodingCache ?? throw new ArgumentNullException(nameof(encodingCache));
        }

        #region Private Fields

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

            RestRouteHandler restRouteHandler = new RestRouteHandler(_configuration, _encodingCache);

            restRouteHandler.RegisterController<V1RequestController>(() =>
            {
                return new object[]
                {
                    _infoDataManager,
                    _receiveManager,
                    _tokenFactory,
                    _universalSendFileManager,
                    _receiveTaskManager,
                    _registerResponseDataManager
                };
            });

            //restRouteHandler.RegisterController<V2RequestController>(); // Register controller
            HttpServerConfiguration = new HttpServerConfiguration();
            HttpServerConfiguration.ListenOnPort(port).RegisterRoute("api/localsend/", restRouteHandler).EnableCors();

            HttpServer = new HttpServer(HttpServerConfiguration, _httpRequestParser);
            try {
                await HttpServer.StartServerAsync();
            } catch {
                return false;
            }

            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v1/send?fileId={}&token={}")) {
                Debug.WriteLine($"[OperationController.UriOperations.Add] /api/localsend/v1/send");
                OperationController.UriOperations.Add(
                    "/api/localsend/v1/send?fileId={}&token={}",
                    _operationFunctions.SendRequestFuncAsync
                );
            }

            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v1/register")) {
                Debug.WriteLine($"[OperationController.UriOperations.Add] /api/localsend/v1/register");
                OperationController.UriOperations.Add(
                    "/api/localsend/v1/register",
                    _operationFunctions.RegisterRequestFunc
                );
            }

            Debug.WriteLine($"HTTP server started on port {port}");
            return true;
        }

        public void StopHttpServer() {
            _udpDiscovery?.StopUdpListener();
            _udpDiscovery = null;

            HttpServer.StopServer();
            Debug.WriteLine($"HTTP server has stopped");
        }

        #endregion Public Methods

    }
}