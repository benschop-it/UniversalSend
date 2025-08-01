using System.Collections.Generic;

namespace UniversalSend.Services.Rest.Models.Contracts {

    internal interface IRestResponse {

        #region Public Properties

        IReadOnlyDictionary<string, string> Headers { get; }
        int StatusCode { get; }

        #endregion Public Properties
    }
}