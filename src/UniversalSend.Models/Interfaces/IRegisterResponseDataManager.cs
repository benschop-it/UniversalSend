namespace UniversalSend.Models.Interfaces {
    public interface IRegisterResponseDataManager {

        #region Public Methods

        IRegisterResponseData DeserializeRegisterResponseData(string json);

        IRegisterResponseData GetRegisterReponseData(bool announcement);

        #endregion Public Methods
    }
}