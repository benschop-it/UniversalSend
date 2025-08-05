namespace UniversalSend.Models.Interfaces {

    public interface IRegisterResponseDataManager {

        #region Public Methods

        IAnnouncementV1 DeserializeAnnouncementV1(string json);

        IAnnouncementV2 DeserializeAnnouncementV2(string json);

        IAnnouncementV1 GetAnnouncementV1(bool announcement);

        IAnnouncementV2 GetAnnouncementV2(bool announcement);

        IRegisterResponseDataV1 DeserializeRegisterResponseDataV1(string json);

        IRegisterResponseDataV2 DeserializeRegisterResponseDataV2(string json);

        IRegisterResponseDataV1 GetRegisterResponseDataV1(bool announcement);

        IRegisterResponseDataV2 GetRegisterResponseDataV2(bool announcement);

        #endregion Public Methods
    }
}