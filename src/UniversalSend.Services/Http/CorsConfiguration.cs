using System.Collections.Generic;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.Http {

    internal class CorsConfiguration : ICorsConfiguration {

        #region Public Properties

        public List<string> AllowedOrigins { get; } = new List<string>();

        #endregion Public Properties

        #region Public Methods

        public ICorsConfiguration AddAllowedOrigin(string allowedOrigin) {
            AllowedOrigins.Add(allowedOrigin);
            return this;
        }

        #endregion Public Methods
    }
}