namespace UniversalSend.Models.Interfaces {

    public interface IRegisterResponseDataManager {

        #region Public Methods

        IAnnouncementV2 DeserializeAnnouncementV2(string json);

        IAnnouncementV2 GetAnnouncementV2(bool announcement);

        IRegisterResponseDataV2 DeserializeRegisterResponseDataV2(string json);

        IRegisterResponseDataV2 GetRegisterResponseDataV2();

        #endregion Public Methods
    }
}