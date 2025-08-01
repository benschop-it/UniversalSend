namespace UniversalSend.Services.Rest {

    internal class PathPart {

        #region Public Enums

        public enum PathPartType {
            Path,
            Argument,
        }

        #endregion Public Enums

        #region Public Constructors

        public PathPart(PathPartType pathPartType, string value) {
            PartType = pathPartType;
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public PathPartType PartType { get; }
        public string Value { get; }

        #endregion Public Properties
    }
}