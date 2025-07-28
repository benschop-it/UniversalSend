namespace UniversalSend.Models.Data {

    public class Device {

        #region Public Properties

        public string Alias { get; set; } = "";
        public string DeviceModel { get; set; } = "";
        public string DeviceType { get; set; } = "";
        public string Fingerprint { get; set; } = "";
        public string HttpProtocol { get; set; } = "http";
        public string IP { get; set; } = "";
        public int Port { get; set; } = -1;
        public string ProtocolVersion { get; set; } = "";

        #endregion Public Properties
    }
}