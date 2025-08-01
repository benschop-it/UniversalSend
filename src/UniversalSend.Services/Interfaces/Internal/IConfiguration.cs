using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IConfiguration {

        #region Public Properties

        MediaType DefaultAcceptType { get; set; }
        MediaType DefaultContentType { get; set; }
        string DefaultJSONCharset { get; set; }
        string DefaultXMLCharset { get; set; }

        #endregion Public Properties
    }
}