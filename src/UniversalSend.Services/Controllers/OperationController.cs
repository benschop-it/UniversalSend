using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniversalSend.Models.Helpers;
using UniversalSend.Services.HttpMessage;

namespace UniversalSend.Services.Controllers {

    internal class OperationController {

        #region Public Properties

        public static Dictionary<string, Func<MutableHttpServerRequest, object>> UriOperations { get; set; } = new Dictionary<string, Func<MutableHttpServerRequest, object>>();

        #endregion Public Properties

        #region Public Methods

        public static void TryRunOperationByRequestUri(MutableHttpServerRequest mutableHttpServerRequest) {
            Debug.WriteLine($"uri:{mutableHttpServerRequest.Uri.ToString()}");
            string uri = StringHelper.GetURLFromURLWithQueryParmeters(mutableHttpServerRequest.Uri.ToString());
            Debug.WriteLine($"Looking for managed function for uri: {uri}");
            if (UriOperations.ContainsKey(uri)) {
                Debug.WriteLine($"Preparing to execute managed function for uri: {uri}");
                Func<MutableHttpServerRequest, object> func = UriOperations[uri];
                func(mutableHttpServerRequest);
            }
        }

        #endregion Public Methods
    }
}