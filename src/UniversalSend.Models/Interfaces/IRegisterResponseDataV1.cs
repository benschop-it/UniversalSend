namespace UniversalSend.Models.Interfaces {

    public interface IRegisterResponseDataV1 {

        #region Public Properties

        string Alias { get; set; }
        string DeviceModel { get; set; }
        string DeviceType { get; set; }
        string Fingerprint { get; set; }

        #endregion Public Properties
    }
}