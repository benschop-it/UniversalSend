using System.Collections.Generic;
using System.Linq;

namespace UniversalSend.Services.Rest {

    internal class ParsedUri {

        #region Public Constructors

        public ParsedUri(IReadOnlyList<PathPart> pathParts, IReadOnlyList<UriParameter> parameters, string fragment) {
            PathParts = pathParts;
            Parameters = parameters;
            Fragment = fragment;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Fragment { get; }
        public IReadOnlyList<UriParameter> Parameters { get; }
        public IReadOnlyList<PathPart> PathParts { get; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString() {
            return $"Path={string.Join("/", PathParts.Select(x => x.PartType == PathPart.PathPartType.Argument ? $"{{{x.Value}}}" : x.Value))}, Parameters={string.Join("&", Parameters.Select(x => $"{x.Name}={x.Value}"))}, Fragment={Fragment}";
        }

        #endregion Public Methods
    }
}