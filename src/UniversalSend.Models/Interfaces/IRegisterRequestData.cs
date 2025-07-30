namespace UniversalSend.Models.Interfaces {
    public interface IRegisterRequestData {
        string Alias { get; set; }
        string DeviceModel { get; set; }
        string DeviceType { get; set; }
        string Fingerprint { get; set; }
    }
}