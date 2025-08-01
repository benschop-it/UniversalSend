namespace UniversalSend.Models.Interfaces {

    public interface IRegisterResponseData {

        #region Public Properties

        string Alias { get; set; }
        bool Announcement { get; set; }
        string DeviceModel { get; set; }
        string DeviceType { get; set; }
        string Fingerprint { get; set; }

        #endregion Public Properties
    }
}