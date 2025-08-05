using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.Common;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Controllers;
using UniversalSend.Services.Helpers;
using UniversalSend.Services.Http;
using UniversalSend.Services.Interfaces;
using UniversalSend.Services.Interfaces.Internal;
using UniversalSend.Services.Rest;
using Windows.UI.Core;

namespace UniversalSend.Services.Misc {

    internal class ServiceHttpServer : IServiceHttpServer {

        #region Private Fields

        private readonly IConfiguration _configuration;
        private readonly IDeviceManager _deviceManager;
        private readonly IEncodingCache _encodingCache;
        private readonly IHttpRequestParser _httpRequestParser;
        private readonly IInfoDataManager _infoDataManager;
        private readonly IInstanceCreatorCache _instanceCreatorCache;
        private readonly ILogger _logger;
        private readonly IOperationFunctions _operationFunctions;
        private readonly IReceiveManager _receiveManager;
        private readonly IReceiveTaskManager _receiveTaskManager;
        private readonly IRegister _register;
        private readonly IRegisterResponseDataManager _registerResponseDataManager;
        private readonly ISettings _settings;
        private readonly ITokenFactory _tokenFactory;
        private readonly IUniversalSendFileManager _universalSendFileManager;
        private readonly CoreDispatcher _dispatcher;
        private UdpDiscoveryService _udpDiscovery;

        #endregion Private Fields

        #region Public Constructors

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
            IEncodingCache encodingCache,
            IInstanceCreatorCache instanceCreatorCache,
            IDispatcherProvider dispatcherProvider
        ) {
            _logger = LogManager.GetLogger<ServiceHttpServer>();
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
            _instanceCreatorCache = instanceCreatorCache ?? throw new ArgumentNullException(nameof(instanceCreatorCache));
            _dispatcher = dispatcherProvider?.Dispatcher ?? throw new ArgumentNullException(nameof(dispatcherProvider));
        }

        #endregion Public Constructors

        #region Internal Properties

        internal HttpServer HttpServer { get; set; }

        internal HttpServerConfiguration HttpServerConfiguration { get; set; }

        #endregion Internal Properties

        #region Public Methods

        public async Task<bool> StartHttpServerAsync(int port) {
            _httpRequestParser.ProgressChanged += OnParserProgressChanged;

            _udpDiscovery = new UdpDiscoveryService(_registerResponseDataManager, _deviceManager, _register, _settings);
            await _udpDiscovery.StartUdpListenerAsync();

            RestRouteHandler restRouteHandler = new RestRouteHandler(_configuration, _encodingCache, _instanceCreatorCache);

            restRouteHandler.RegisterController<V2RequestController>(() => {
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
                _httpRequestParser.ProgressChanged -= OnParserProgressChanged;
                return false;
            }

            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v1/send?fileId={}&token={}")) {
                _logger.Debug($"[OperationController.UriOperations.Add] /api/localsend/v1/send");
                OperationController.UriOperations.Add(
                    "/api/localsend/v1/send?fileId={}&token={}",
                    _operationFunctions.SendRequestFuncAsync
                );
            }

            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v1/register")) {
                _logger.Debug($"[OperationController.UriOperations.Add] /api/localsend/v1/register");
                OperationController.UriOperations.Add(
                    "/api/localsend/v1/register",
                    _operationFunctions.RegisterRequestFuncV1
                );
            }

            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v2/upload?sessionId={}&fileId={}&token={}")) {
                _logger.Debug($"[OperationController.UriOperations.Add] /api/localsend/v2/upload");
                OperationController.UriOperations.Add(
                    "/api/localsend/v2/upload?sessionId={}&fileId={}&token={}",
                    _operationFunctions.UploadRequestFuncAsync
                );
            }

            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v2/register")) {
                _logger.Debug($"[OperationController.UriOperations.Add] /api/localsend/v2/register");
                OperationController.UriOperations.Add(
                    "/api/localsend/v2/register",
                    _operationFunctions.RegisterRequestFuncV2
                );
            }

            _logger.Debug($"HTTP server started on port {port}");
            return true;
        }

        public void StopHttpServer() {
            _udpDiscovery?.StopUdpListener();
            _udpDiscovery = null;

            _httpRequestParser.ProgressChanged -= OnParserProgressChanged;

            HttpServer.StopServer();
            _logger.Debug("HTTP server has stopped");
        }

        #endregion Public Methods

        #region Private Methods

        private async void OnParserProgressChanged(object sender, HttpParseProgressEventArgs e) {
            Dictionary<string, string> parameters = StringHelper.GetURLQueryParameters(e.Uri.ToString());
            if (parameters.Count > 0) {
                if (parameters.ContainsKey("fileId") && parameters.ContainsKey("token")) {
                    var fileId = parameters["fileId"];
                    var token = parameters["token"];

                    var receivingTasks = _receiveTaskManager.ReceivingTasks;

                    foreach (IReceiveTask task in receivingTasks) {
                        IUniversalSendFileV1 file = task.FileV1;
                        if (file.Id == fileId && file.TransferToken == token) {
                            if (_dispatcher.HasThreadAccess) {
                                UpdateTask(task, e);
                            } else {
                                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => UpdateTask(task, e));
                            }
                            break;
                        }
                    }
                }
            }

            _receiveManager.SendProgressEvent(e);
        }

        private void UpdateTask(IReceiveTask task, HttpParseProgressEventArgs e) {
            task.Error = e.Error;
            task.Progress = e.Percent ?? 0;
            task.Status = e.Status;
        }

        #endregion Private Methods

    }
}