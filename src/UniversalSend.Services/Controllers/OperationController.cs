using System;
using System.Collections.Generic;
using UniversalSend.Models.Common;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Helpers;
using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Controllers {

    internal class OperationController {

        private static readonly ILogger _logger;

        static OperationController() {
            _logger = LogManager.GetLogger<OperationController>();
        }

        #region Public Properties

        public static Dictionary<string, Func<MutableHttpServerRequest, object>> UriOperations { get; set; } = new Dictionary<string, Func<MutableHttpServerRequest, object>>();

        #endregion Public Properties

        #region Public Methods

        public static void TryRunOperationByRequestUri(MutableHttpServerRequest mutableHttpServerRequest) {
            //_logger.Debug($"TryRunOperationByRequestUri for URI:{mutableHttpServerRequest.Uri.ToString()}.");
            string uri = StringHelper.GetURLFromURLWithQueryParmeters(mutableHttpServerRequest.Uri.ToString());
            _logger.Debug($"TryRunOperationByRequestUri: raw='{mutableHttpServerRequest.Uri}', normalized='{uri}', registered=[{string.Join(", ", UriOperations.Keys)}]");
            if (UriOperations.ContainsKey(uri)) {
                _logger.Debug($"TryRunOperationByRequestUri: executing side-effect handler for '{uri}'.");
                Func<MutableHttpServerRequest, object> func = UriOperations[uri];
                func(mutableHttpServerRequest);
            } else {
                _logger.Debug($"TryRunOperationByRequestUri: no side-effect handler matched '{uri}'.");
            }
        }

        #endregion Public Methods

    }
}