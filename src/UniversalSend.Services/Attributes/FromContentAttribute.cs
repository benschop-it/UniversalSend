using System;

namespace UniversalSend.Services.Attributes {

    /// <summary>
    /// This class is only used as a marker
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    internal sealed class FromContentAttribute : Attribute {
    }
}