using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services {
    internal class Configuration
    {
        internal static Configuration Default { get; }

        static Configuration()
        {
            Default = new Configuration();
        }

        public Configuration()
        {
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
