namespace UniversalSend.Models.Interfaces {
    public interface IFavorite {
        string DeviceName { get; set; }
        string IPAddr { get; set; }
        long Port { get; set; }
    }
}