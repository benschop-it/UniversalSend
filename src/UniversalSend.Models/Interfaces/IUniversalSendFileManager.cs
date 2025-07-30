using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Data {
    public interface IUniversalSendFileManager {
        IUniversalSendFile CreateUniversalSendFileFromText(string text);
        IUniversalSendFile GetUniversalSendFileFromFileRequestData(IFileRequestData fileRequestData);
        IUniversalSendFile GetUniversalSendFileFromFileRequestDataAndToken(IFileRequestData fileRequestData, string token);
    }
}