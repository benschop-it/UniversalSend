namespace UniversalSend.Models.Interfaces {
    public interface IDevice {
        string Alias { get; set; }
        string DeviceModel { get; set; }
        string DeviceType { get; set; }
        string Fingerprint { get; set; }
        string HttpProtocol { get; set; }
        string IP { get; set; }
        int Port { get; set; }
        string ProtocolVersion { get; set; }
    }
}