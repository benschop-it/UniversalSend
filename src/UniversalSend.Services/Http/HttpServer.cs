using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniversalSend.Models.Common;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.HttpMessage.Headers.Response;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.Interfaces.Internal;
using Windows.Networking.Sockets;

namespace UniversalSend.Services.Http {

    internal class HttpServer : IDisposable {

        #region Private Fields

        private readonly ContentEncoderFactory _contentEncoderFactory;
        private readonly IHttpRequestParser _httpRequestParser;
        private readonly StreamSocketListener _listener;
        private readonly ILogger _log;
        private readonly List<IHttpMessageInspector> _messageInspectors;
        private readonly int _port;
        private readonly SortedSet<RouteRegistration> _routes;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        #endregion Private Fields

        #region Public Constructors

        public HttpServer(
                    HttpServerConfiguration configuration,
            IHttpRequestParser httpRequestParser
        ) {
            _log = LogManager.GetLogger<HttpServer>();
            _port = configuration.ServerPort;
            _listener = new StreamSocketListener();

            _listener.ConnectionReceived += ProcessRequestAsync;
            _contentEncoderFactory = new ContentEncoderFactory();
            _messageInspectors = new List<IHttpMessageInspector>();

            if (configuration.CorsConfiguration != null)
                _messageInspectors.Add(new CorsMessageInspector(configuration.CorsConfiguration.AllowedOrigins));

            _routes = new SortedSet<RouteRegistration>(configuration.Routes);
            _httpRequestParser = httpRequestParser ?? throw new ArgumentNullException(nameof(httpRequestParser));
        }

        #endregion Public Constructors

        #region Public Methods

        void IDisposable.Dispose() {
            _listener.Dispose();
        }

        public async Task StartServerAsync() {
            await _listener.BindServiceNameAsync(_port.ToString());

            _log.Info($"Webserver listening on port {_port}");
        }

        public void StopServer() {
            ((IDisposable)this).Dispose();

            _log.Info($"Webserver stopped listening on port {_port}");
        }

        #endregion Public Methods

        #region Internal Methods

        internal async Task<HttpServerResponse> HandleRequestAsync(IHttpServerRequest request) {
            var routeRegistration = _routes.FirstOrDefault(x => x.Match(request));
            if (routeRegistration == null) {
                return HttpServerResponse.Create(new Version(1, 1), HttpResponseStatus.BadRequest);
            }

            var httpResponse = ApplyMessageInspectorsBeforeHandleRequest(request);

            if (httpResponse == null)
                httpResponse = await routeRegistration.HandleAsync(request);

            httpResponse = await AddContentEncodingAsync(httpResponse, request.AcceptEncodings);
            httpResponse = ApplyMessageInspectorsAfterHandleRequest(request, httpResponse);

            return httpResponse;
        }

        #endregion Internal Methods

        #region Private Methods

        private static void AddHeaderIfNotNull(IHttpHeader contentEncodingHeader, HttpServerResponse newResponse) {
            if (contentEncodingHeader != null) {
                newResponse.AddHeader(contentEncodingHeader);
            }
        }

        private static async Task WriteResponseAsync(HttpServerResponse response, StreamSocket socket) {
            using (var output = socket.OutputStream) {
                await output.WriteAsync(response.ToBytes().AsBuffer());
                await output.FlushAsync();
            }
        }

        private async Task<HttpServerResponse> AddContentEncodingAsync(HttpServerResponse httpResponse, IEnumerable<string> acceptEncodings) {
            var contentEncoder = _contentEncoderFactory.GetEncoder(acceptEncodings);
            var encodedContent = await contentEncoder.Encode(httpResponse.Content);

            var newResponse = HttpServerResponse.Create(httpResponse.HttpVersion, httpResponse.ResponseStatus);

            foreach (var header in httpResponse.Headers) {
                newResponse.AddHeader(header);
            }
            newResponse.Content = encodedContent;
            newResponse.AddHeader(new ContentLengthHeader(encodedContent?.Length ?? 0));

            var contentEncodingHeader = contentEncoder.ContentEncodingHeader;
            AddHeaderIfNotNull(contentEncodingHeader, newResponse);

            return newResponse;
        }

        private HttpServerResponse ApplyMessageInspectorsAfterHandleRequest(IHttpServerRequest request,
            HttpServerResponse httpResponse) {
            foreach (var httpMessageInspector in _messageInspectors) {
                var result = httpMessageInspector.AfterHandleRequest(request, httpResponse);
                if (result != null)
                    httpResponse = result.Response;
            }
            return httpResponse;
        }

        private HttpServerResponse ApplyMessageInspectorsBeforeHandleRequest(IHttpServerRequest request) {
            foreach (var httpMessageInspector in _messageInspectors) {
                var result = httpMessageInspector.BeforeHandleRequest(request);
                if (result != null)
                    return result.Response;
            }

            return null;
        }

        private async void ProcessRequestAsync(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args) {
            await Task.Run(async () => {
                var requestLog = new StringBuilder(); // Local log buffer

                try {
                    using (var inputStream = args.Socket.InputStream) {
                        MutableHttpServerRequest request = await _httpRequestParser.ParseRequestStream(inputStream) as MutableHttpServerRequest;

                        requestLog.AppendLine("[HttpServer.ProcessRequestAsync] [Request]");
                        requestLog.AppendLine($"    Uri = {request.Uri}");

                        foreach (var header in request.Headers) {
                            requestLog.AppendLine($"    Header = {header.Name}: {header.Value}");
                        }

                        var httpResponse = await HandleRequestAsync(request);

                        requestLog.AppendLine($"[HttpServer.ProcessRequestAsync] [Response]");
                        requestLog.AppendLine($"    ContentType = {httpResponse.ContentType}");

                        foreach (var header in httpResponse.Headers)
                            requestLog.AppendLine($"    {header.Name}: {header.Value}");

                        requestLog.AppendLine($"    HttpResponse = {httpResponse}");

                        await WriteResponseAsync(httpResponse, args.Socket);
                    }
                } catch (Exception ex) {
                    requestLog.AppendLine($"[HttpServer.ProcessRequestAsync] Exception: {ex.Message}");
                } finally {
                    try { args.Socket.Dispose(); } catch { }

                    // Emit full log block at once
                    Debug.WriteLine(requestLog.ToString());
                }
            });
        }

        #endregion Private Methods
    }
}