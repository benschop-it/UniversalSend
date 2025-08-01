using System.Collections.Generic;
using UniversalSend.Services.HttpMessage.Models.Schemas;

namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class AllowHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Allow";

        #endregion Internal Fields

        #region Public Constructors

        public AllowHeader(IEnumerable<HttpMethod> allows) : base(NAME, string.Join(";", allows)) {
            Allows = allows;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<HttpMethod> Allows { get; }

        #endregion Public Properties
    }
}