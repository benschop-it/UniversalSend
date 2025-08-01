using System.Collections.Generic;

namespace UniversalSend.Services.Rest {

    internal class MatchUri {

        #region Public Constructors

        public MatchUri(string path, IReadOnlyCollection<string> pathParameters, IReadOnlyCollection<string> uriParameters) {
            Path = path;
            PathParameters = pathParameters;
            UriParameters = uriParameters;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Path { get; }
        public IReadOnlyCollection<string> PathParameters { get; }
        public IReadOnlyCollection<string> UriParameters { get; }

        #endregion Public Properties
    }
}