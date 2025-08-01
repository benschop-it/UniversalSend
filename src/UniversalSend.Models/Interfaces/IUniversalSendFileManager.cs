namespace UniversalSend.Models.Interfaces {
    public interface IUniversalSendFileManager {

        #region Public Methods

        IUniversalSendFile CreateUniversalSendFileFromText(string text);
        IUniversalSendFile GetUniversalSendFileFromFileRequestData(IFileRequestData fileRequestData);
        IUniversalSendFile GetUniversalSendFileFromFileRequestDataAndToken(IFileRequestData fileRequestData, string token);

        #endregion Public Methods
    }
}