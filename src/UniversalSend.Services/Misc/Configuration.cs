﻿using UniversalSend.Services.Interfaces.Internal;
using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services.Misc {

    internal class Configuration : IConfiguration {

        #region Public Constructors

        public Configuration() {
            DefaultAcceptType = MediaType.JSON;
            DefaultContentType = MediaType.JSON;

            DefaultJSONCharset = "utf-8";
            DefaultXMLCharset = "utf-8";
        }

        #endregion Public Constructors

        #region Public Properties

        public MediaType DefaultAcceptType { get; set; }
        public MediaType DefaultContentType { get; set; }

        public string DefaultJSONCharset { get; set; }
        public string DefaultXMLCharset { get; set; }

        #endregion Public Properties
    }
}