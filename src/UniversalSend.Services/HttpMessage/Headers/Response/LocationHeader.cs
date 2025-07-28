using System;

namespace UniversalSend.Services.HttpMessage.Headers.Response {

    public class LocationHeader : HttpHeaderBase {
        internal static string NAME = "Location";

        public Uri Location { get; }

        public LocationHeader(Uri location) : base(NAME, location.ToString()) {
            Location = location;
        }
    }
}