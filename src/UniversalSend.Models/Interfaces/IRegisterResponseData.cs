namespace UniversalSend.Models.Interfaces {
    public interface IRegisterResponseData {
        string Alias { get; set; }
        bool Announcement { get; set; }
        string DeviceModel { get; set; }
        string DeviceType { get; set; }
        string Fingerprint { get; set; }
    }
}