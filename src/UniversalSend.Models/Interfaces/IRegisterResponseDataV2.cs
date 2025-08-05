using Newtonsoft.Json;

namespace UniversalSend.Models.Interfaces {

    public interface IRegisterResponseDataV2 {

        #region Public Properties

        string Alias { get; set; }
        string Version { get; set; }
        string DeviceModel { get; set; }
        string DeviceType { get; set; }
        string Fingerprint { get; set; }
        bool Download { get; set; }

        #endregion Public Properties
    }
}