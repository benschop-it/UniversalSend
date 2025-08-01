namespace UniversalSend.Models.Interfaces {
    public interface IFavorite {

        #region Public Properties

        string DeviceName { get; set; }
        string IPAddr { get; set; }
        long Port { get; set; }

        #endregion Public Properties
    }
}