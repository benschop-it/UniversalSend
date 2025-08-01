namespace UniversalSend.Models.Interfaces {

    public interface IInfoData {

        #region Public Properties

        string Alias { get; set; }
        string DeviceModel { get; set; }
        string DeviceType { get; set; }

        #endregion Public Properties
    }
}