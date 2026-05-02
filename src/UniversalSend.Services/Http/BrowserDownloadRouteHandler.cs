using System;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.Misc;
using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.Http {

    internal class BrowserDownloadRouteHandler : IRouteHandler {

        private readonly IWebSendManager _webSendManager;

        public BrowserDownloadRouteHandler(IWebSendManager webSendManager) {
            _webSendManager = webSendManager ?? throw new ArgumentNullException(nameof(webSendManager));
        }

        public Task<HttpServerResponse> HandleRequest(UniversalSend.Services.Interfaces.Internal.IHttpServerRequest request) {
            if (request?.Uri == null || request.Method != HttpMethod.GET) {
                return Task.FromResult(HttpServerResponse.Create(HttpResponseStatus.NotFound));
            }

            var relativeUri = request.Uri.ToRelativeString();
            if (!string.IsNullOrEmpty(relativeUri)) {
                return Task.FromResult(HttpServerResponse.Create(HttpResponseStatus.NotFound));
            }

            var share = _webSendManager.GetActiveShare();
            if (share == null) {
                return Task.FromResult(HttpServerResponse.Create(HttpResponseStatus.NotFound));
            }

            var response = HttpServerResponse.Create(HttpResponseStatus.Found);
            response.Location = new Uri("/api/localsend/v2/browser-download", UriKind.Relative);
            response.Date = DateTime.Now;
            response.IsConnectionClosed = true;
            return Task.FromResult(response);
        }
    }
}
