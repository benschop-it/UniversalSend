using UniversalSend.Services.Models.Schemas;
using System;

namespace UniversalSend.Services.Attributes {
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RestControllerAttribute : Attribute
    {
        public InstanceCreationType InstanceCreationType { get; }

        public RestControllerAttribute(InstanceCreationType instanceCreation)
        {
            InstanceCreationType = instanceCreation;
        }
    }
}
