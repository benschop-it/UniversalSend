using System;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using UniversalSend.Services.HttpMessage;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.Misc;
using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.Http {

    internal class BrowserDownloadRouteHandler : IRouteHandler {

        private readonly IContentDialogManager _contentDialogManager;
        private readonly ISettings _settings;
        private readonly IWebSendManager _webSendManager;

        public BrowserDownloadRouteHandler(IWebSendManager webSendManager, ISettings settings, IContentDialogManager contentDialogManager) {
            _webSendManager = webSendManager ?? throw new ArgumentNullException(nameof(webSendManager));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _contentDialogManager = contentDialogManager ?? throw new ArgumentNullException(nameof(contentDialogManager));
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

            return HandleBrowserRequestAsync(request.RemoteAddress);
        }

        private async Task<HttpServerResponse> HandleBrowserRequestAsync(string remoteAddress) {
            var share = _webSendManager.GetActiveShare();
            if (share == null) {
                return HttpServerResponse.Create(HttpResponseStatus.NotFound);
            }

            if (!_webSendManager.IsClientApproved(remoteAddress)) {
                var autoAcceptSetting = _settings.GetSettingContent(Constants.WebShare_AutoAccept);
                bool autoAccept = autoAcceptSetting is bool autoAcceptBool ? autoAcceptBool : bool.TryParse(autoAcceptSetting?.ToString(), out bool autoAcceptParsed) && autoAcceptParsed;

                if (!autoAccept) {
                    var result = await _contentDialogManager.ShowContentDialogAsync(
                        "Share via Link",
                        $"Allow browser download request from {remoteAddress ?? "unknown device"}?",
                        "Allow",
                        "Reject",
                        null);

                    if (result != Windows.UI.Xaml.Controls.ContentDialogResult.Primary) {
                        return CreateTextResponse(HttpResponseStatus.Forbidden, "The browser download request was rejected.");
                    }
                }

                _webSendManager.ApproveClient(remoteAddress);
            }

            var response = HttpServerResponse.Create(HttpResponseStatus.Found);
            response.Location = new Uri("/api/localsend/v2/browser-download", UriKind.Relative);
            response.Date = DateTime.Now;
            response.IsConnectionClosed = true;
            return response;
        }

        private static HttpServerResponse CreateTextResponse(HttpResponseStatus status, string content) {
            var response = HttpServerResponse.Create(status);
            response.ContentType = "text/plain";
            response.ContentCharset = "utf-8";
            response.Content = Encoding.UTF8.GetBytes(content ?? string.Empty);
            response.Date = DateTime.Now;
            response.IsConnectionClosed = true;
            return response;
        }
    }
}
