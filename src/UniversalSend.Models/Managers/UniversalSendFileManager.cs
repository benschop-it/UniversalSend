using System;
using System.Text;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    internal class UniversalSendFileManager : IUniversalSendFileManager {

        #region Public Methods

        public IUniversalSendFileV1 CreateUniversalSendFileFromTextV1(string text) {
            UniversalSendFileV1 universalSendFile = new UniversalSendFileV1();
            universalSendFile.Id = Guid.NewGuid().ToString();
            universalSendFile.FileName = universalSendFile.Id + ".txt";
            universalSendFile.Size = Encoding.ASCII.GetBytes(text).Length;
            universalSendFile.FileType = "text";
            universalSendFile.Preview = text;
            return universalSendFile;
        }

        public IUniversalSendFileV2 CreateUniversalSendFileFromTextV2(string text) {
            UniversalSendFileV2 universalSendFile = new UniversalSendFileV2();
            universalSendFile.Id = Guid.NewGuid().ToString();
            universalSendFile.FileName = universalSendFile.Id + ".txt";
            universalSendFile.Size = Encoding.ASCII.GetBytes(text).Length;
            universalSendFile.FileType = "text";
            universalSendFile.Sha256 = null;
            universalSendFile.Preview = text;
            return universalSendFile;
        }

        public IUniversalSendFileV1 GetUniversalSendFileFromFileRequestDataV1(IFileRequestDataV1 fileRequestData) {
            UniversalSendFileV1 universalSendFile = new UniversalSendFileV1();
            universalSendFile.Id = fileRequestData.Id;
            universalSendFile.FileName = fileRequestData.FileName;
            universalSendFile.Size = fileRequestData.Size;
            universalSendFile.FileType = fileRequestData.FileType;
            universalSendFile.Preview = fileRequestData.Preview;
            return universalSendFile;
        }

        public IUniversalSendFileV2 GetUniversalSendFileFromFileRequestDataV2(IFileRequestDataV2 fileRequestData) {
            UniversalSendFileV2 universalSendFile = new UniversalSendFileV2();
            universalSendFile.Id = fileRequestData.Id;
            universalSendFile.FileName = fileRequestData.FileName;
            universalSendFile.Size = fileRequestData.Size;
            universalSendFile.FileType = fileRequestData.FileType;
            universalSendFile.Sha256 = fileRequestData.Sha256;
            universalSendFile.Preview = fileRequestData.Preview;

            return universalSendFile;
        }

        public IUniversalSendFileV1 GetUniversalSendFileFromFileRequestDataV1AndToken(IFileRequestDataV1 fileRequestData, string token) {
            UniversalSendFileV1 universalSendFile = new UniversalSendFileV1();
            universalSendFile.Id = fileRequestData.Id;
            universalSendFile.FileName = fileRequestData.FileName;
            universalSendFile.Size = fileRequestData.Size;
            universalSendFile.FileType = fileRequestData.FileType;
            universalSendFile.Preview = fileRequestData.Preview;
            universalSendFile.TransferToken = token;
            return universalSendFile;
        }

        public IUniversalSendFileV2 GetUniversalSendFileFromFileRequestDataV2AndToken(IFileRequestDataV2 fileRequestData, string token) {
            UniversalSendFileV2 universalSendFile = new UniversalSendFileV2();
            universalSendFile.Id = fileRequestData.Id;
            universalSendFile.FileName = fileRequestData.FileName;
            universalSendFile.Size = fileRequestData.Size;
            universalSendFile.FileType = fileRequestData.FileType;
            universalSendFile.Sha256 = fileRequestData.Sha256;
            universalSendFile.Preview = fileRequestData.Preview;
            universalSendFile.TransferToken = token;
            return universalSendFile;
        }

        #endregion Public Methods

    }
}