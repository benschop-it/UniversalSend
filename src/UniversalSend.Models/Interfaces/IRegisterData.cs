namespace UniversalSend.Models.Interfaces {
    public interface IRegisterData {
        string Alias { get; set; }
        bool Announce { get; set; }
        string DeviceModel { get; set; }
        string DeviceType { get; set; }
        bool Download { get; set; }
        string Fingerprint { get; set; }
        int Port { get; set; }
        string Protocol { get; set; }
        string Version { get; set; }
    }
}