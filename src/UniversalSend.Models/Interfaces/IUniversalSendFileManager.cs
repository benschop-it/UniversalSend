namespace UniversalSend.Models.Interfaces {
    public interface IUniversalSendFileManager {

        #region Public Methods

        IUniversalSendFileV1 CreateUniversalSendFileFromTextV1(string text);
        IUniversalSendFileV2 CreateUniversalSendFileFromTextV2(string text);
        IUniversalSendFileV1 GetUniversalSendFileFromFileRequestDataV1(IFileRequestDataV1 fileRequestData);
        IUniversalSendFileV2 GetUniversalSendFileFromFileRequestDataV2(IFileRequestDataV2 fileRequestData);
        IUniversalSendFileV1 GetUniversalSendFileFromFileRequestDataV1AndToken(IFileRequestDataV1 fileRequestData, string token);
        IUniversalSendFileV2 GetUniversalSendFileFromFileRequestDataV2AndToken(IFileRequestDataV2 fileRequestData, string token);

        #endregion Public Methods
    }
}