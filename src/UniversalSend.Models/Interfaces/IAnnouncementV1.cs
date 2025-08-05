namespace UniversalSend.Models.Interfaces {

    public interface IAnnouncementV1 : IRegisterResponseDataV1 {

        #region Public Properties

        bool Announcement { get; set; }

        #endregion Public Properties
    }
}