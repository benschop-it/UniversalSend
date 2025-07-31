using System;
using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services.Attributes {

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class RestControllerAttribute : Attribute {

        #region Public Constructors

        public RestControllerAttribute(InstanceCreationType instanceCreation) {
            InstanceCreationType = instanceCreation;
        }

        #endregion Public Constructors

        #region Public Properties

        public InstanceCreationType InstanceCreationType { get; }

        #endregion Public Properties
    }
}