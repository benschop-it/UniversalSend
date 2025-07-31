using System;
using System.Text;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {
    public class UniversalSendFileManager : IUniversalSendFileManager {

        #region Public Methods

        public IUniversalSendFile CreateUniversalSendFileFromText(string text) {
            UniversalSendFile universalSendFile = new UniversalSendFile();
            universalSendFile.FileType = "text";
            universalSendFile.Text = text;
            universalSendFile.Size = Encoding.ASCII.GetBytes(text).Length;
            universalSendFile.Id = Guid.NewGuid().ToString();
            universalSendFile.FileName = universalSendFile.Id + ".txt";
            return universalSendFile;
        }

        public IUniversalSendFile GetUniversalSendFileFromFileRequestData(IFileRequestData fileRequestData) {
            UniversalSendFile universalSendFile = new UniversalSendFile();
            universalSendFile.Id = fileRequestData.Id;
            universalSendFile.FileName = fileRequestData.FileName;
            universalSendFile.FileType = fileRequestData.FileType;
            universalSendFile.Size = fileRequestData.Size;
            return universalSendFile;
        }

        public IUniversalSendFile GetUniversalSendFileFromFileRequestDataAndToken(IFileRequestData fileRequestData, string token) {
            UniversalSendFile universalSendFile = new UniversalSendFile();
            universalSendFile.Id = fileRequestData.Id;
            universalSendFile.FileName = fileRequestData.FileName;
            universalSendFile.FileType = fileRequestData.FileType;
            universalSendFile.Size = fileRequestData.Size;
            universalSendFile.TransferToken = token;
            return universalSendFile;
        }

        #endregion Public Methods
    }
}