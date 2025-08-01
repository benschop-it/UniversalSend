namespace UniversalSend.Models.Interfaces {
    public interface IUniversalSendFileManager {
        IUniversalSendFile CreateUniversalSendFileFromText(string text);
        IUniversalSendFile GetUniversalSendFileFromFileRequestData(IFileRequestData fileRequestData);
        IUniversalSendFile GetUniversalSendFileFromFileRequestDataAndToken(IFileRequestData fileRequestData, string token);
    }
}