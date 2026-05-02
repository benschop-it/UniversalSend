using System;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using UniversalSend.Services.Helpers;
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

            var requestUri = request.Uri.ToString();
            int queryIndex = requestUri.IndexOf('?');
            var requestPath = queryIndex >= 0 ? requestUri.Substring(0, queryIndex) : requestUri;
            if (string.IsNullOrEmpty(requestPath)) {
                requestPath = "/";
            }

            if (!string.Equals(requestPath, "/", StringComparison.OrdinalIgnoreCase)) {
                return Task.FromResult(HttpServerResponse.Create(HttpResponseStatus.NotFound));
            }

            var share = _webSendManager.GetActiveShare();
            if (share == null) {
                return Task.FromResult(HttpServerResponse.Create(HttpResponseStatus.NotFound));
            }

            return HandleBrowserRequestAsync(request);
        }

        private async Task<HttpServerResponse> HandleBrowserRequestAsync(UniversalSend.Services.Interfaces.Internal.IHttpServerRequest request) {
            var share = _webSendManager.GetActiveShare();
            if (share == null) {
                return HttpServerResponse.Create(HttpResponseStatus.NotFound);
            }

            string remoteAddress = request.RemoteAddress;
            var queryParameters = StringHelper.GetURLQueryParameters(request.Uri.ToString());
            queryParameters.TryGetValue("pin", out string pin);

            var pinResult = _webSendManager.ValidatePin(pin);
            if (pinResult == WebSendPinResult.InvalidPin) {
                return CreateHtmlResponse(HttpResponseStatus.Unauthorized, BuildPinPage(!string.IsNullOrWhiteSpace(pin), false));
            }

            if (pinResult == WebSendPinResult.TooManyAttempts) {
                return CreateHtmlResponse(HttpResponseStatus.Forbidden, BuildPinPage(true, true));
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

        private static string BuildPinPage(bool showInvalidPin, bool tooManyAttempts) {
            var html = new StringBuilder();
            html.Append("<!DOCTYPE html><html><head><meta charset=\"utf-8\" /><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" />");
            html.Append("<title>UniversalSend PIN</title>");
            html.Append("<style>body{font-family:Segoe UI,Arial,sans-serif;margin:24px;}form{max-width:320px;}input{width:100%;padding:8px;margin:12px 0;box-sizing:border-box;}button{padding:8px 16px;}p.error{color:#c62828;}</style>");
            html.Append("</head><body>");
            html.Append("<h1>Enter PIN</h1>");
            html.Append("<p>This share requires a PIN before access is granted.</p>");

            if (tooManyAttempts) {
                html.Append("<p class=\"error\">Too many incorrect PIN attempts.</p>");
            } else if (showInvalidPin) {
                html.Append("<p class=\"error\">Invalid PIN.</p>");
            }

            if (!tooManyAttempts) {
                html.Append("<form method=\"get\" action=\"/\">");
                html.Append("<input type=\"password\" name=\"pin\" placeholder=\"Enter PIN\" autofocus />");
                html.Append("<button type=\"submit\">Continue</button>");
                html.Append("</form>");
            }

            html.Append("</body></html>");
            return html.ToString();
        }

        private static HttpServerResponse CreateHtmlResponse(HttpResponseStatus status, string content) {
            var response = HttpServerResponse.Create(status);
            response.ContentType = "text/html";
            response.ContentCharset = "utf-8";
            response.Content = Encoding.UTF8.GetBytes(content ?? string.Empty);
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
