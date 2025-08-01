namespace UniversalSend.Services.Rest {

    internal class UriParameter {

        #region Public Constructors

        public UriParameter(string name) {
            Name = name;
        }

        public UriParameter(string name, string value) {
            Name = name;
            Value = value;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }
        public string Value { get; }

        #endregion Public Properties
    }
}