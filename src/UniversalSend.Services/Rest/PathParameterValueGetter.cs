using System;

namespace UniversalSend.Services.Rest {

    internal class PathParameterValueGetter : ParameterValueGetter {

        #region Private Fields

        private readonly int _pathIndex;

        #endregion Private Fields

        #region Public Constructors

        public PathParameterValueGetter(string methodName, Type parameterType, int pathIndex) : base(methodName, parameterType) {
            _pathIndex = pathIndex;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override string GetValueFromUri(ParsedUri parsedUri) {
            return parsedUri.PathParts[_pathIndex].Value;
        }

        #endregion Protected Methods
    }
}