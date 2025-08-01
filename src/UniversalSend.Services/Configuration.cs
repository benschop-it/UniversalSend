using UniversalSend.Services.Interfaces;
using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services {

    public class Configuration : IConfiguration {
        public Configuration() {
            DefaultAcceptType = MediaType.JSON;
            DefaultContentType = MediaType.JSON;

            DefaultJSONCharset = "utf-8";
            DefaultXMLCharset = "utf-8";
        }

        public MediaType DefaultAcceptType { get; set; }
        public MediaType DefaultContentType { get; set; }

        public string DefaultJSONCharset { get; set; }
        public string DefaultXMLCharset { get; set; }
    }
}