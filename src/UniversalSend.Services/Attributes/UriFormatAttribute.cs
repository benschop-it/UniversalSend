using System;

namespace UniversalSend.Services.Attributes {

    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class UriFormatAttribute : Attribute {

        #region Public Constructors

        public UriFormatAttribute(string uriFormat) {
            this.UriFormat = uriFormat;
        }

        #endregion Public Constructors

        #region Public Properties

        public string UriFormat { get; }

        #endregion Public Properties
    }
}