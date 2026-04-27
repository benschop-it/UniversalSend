namespace UniversalSend.Models.Interfaces {
    public interface IRegisterRequestDataV2 {

        #region Public Properties

        string Alias { get; set; }
        string Version { get; set; }
        string DeviceModel { get; set; }
        string DeviceType { get; set; }
        string Fingerprint { get; set; }
        int Port { get; set; }
        string Protocol { get; set; }
        bool Download { get; set; }

        #endregion Public Properties
    }
}