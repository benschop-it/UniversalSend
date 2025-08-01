using System.Collections.Generic;
using UniversalSend.Services.HttpMessage.Models.Schemas;

namespace UniversalSend.Services.Models.Schemas {

    internal class MethodNotAllowedResponse : StatusOnlyResponse {

        #region Internal Constructors

        internal MethodNotAllowedResponse(IEnumerable<HttpMethod> allows) : base(405) {
            Allows = allows;
        }

        #endregion Internal Constructors

        #region Public Properties

        public IEnumerable<HttpMethod> Allows { get; }

        #endregion Public Properties
    }
}