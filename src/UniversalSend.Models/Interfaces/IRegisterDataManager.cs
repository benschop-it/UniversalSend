namespace UniversalSend.Models.Interfaces {
    public interface IRegisterDataManager {

        #region Public Methods

        IRegisterDataV2 GetRegisterDataV2FromDevice();

        #endregion Public Methods
    }
}