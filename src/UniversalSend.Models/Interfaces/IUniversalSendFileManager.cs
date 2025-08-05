namespace UniversalSend.Models.Interfaces {
    public interface IUniversalSendFileManager {

        #region Public Methods

        IUniversalSendFileV2 CreateUniversalSendFileFromTextV2(string text);
        IUniversalSendFileV2 GetUniversalSendFileFromFileRequestDataV2(IFileRequestDataV2 fileRequestData);
        IUniversalSendFileV2 GetUniversalSendFileFromFileRequestDataV2AndToken(IFileRequestDataV2 fileRequestData, string token);

        #endregion Public Methods
    }
}