using System.Collections.Generic;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.Http {

    internal class CorsConfiguration : ICorsConfiguration {
        public List<string> AllowedOrigins { get; } = new List<string>();

        public ICorsConfiguration AddAllowedOrigin(string allowedOrigin) {
            AllowedOrigins.Add(allowedOrigin);
            return this;
        }
    }
}