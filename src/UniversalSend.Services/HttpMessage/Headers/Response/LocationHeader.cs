using System;

namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class LocationHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Location";

        #endregion Internal Fields

        #region Public Constructors

        public LocationHeader(Uri location) : base(NAME, location.ToString()) {
            Location = location;
        }

        #endregion Public Constructors

        #region Public Properties

        public Uri Location { get; }

        #endregion Public Properties
    }
}