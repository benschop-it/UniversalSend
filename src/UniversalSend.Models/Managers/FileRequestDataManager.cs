using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using Windows.Storage;

namespace UniversalSend.Models.Managers {

    internal class FileRequestDataManager : IFileRequestDataManager {

        #region Private Fields

        private static readonly Dictionary<string, string> ContentTypeByExtension = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
            [".txt"] = "text/plain",
            [".json"] = "application/json",
            [".xml"] = "application/xml",
            [".html"] = "text/html",
            [".htm"] = "text/html",
            [".csv"] = "text/csv",
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".png"] = "image/png",
            [".gif"] = "image/gif",
            [".bmp"] = "image/bmp",
            [".webp"] = "image/webp",
            [".svg"] = "image/svg+xml",
            [".mp4"] = "video/mp4",
            [".m4v"] = "video/x-m4v",
            [".mov"] = "video/quicktime",
            [".avi"] = "video/x-msvideo",
            [".mkv"] = "video/x-matroska",
            [".mp3"] = "audio/mpeg",
            [".wav"] = "audio/wav",
            [".m4a"] = "audio/mp4",
            [".ogg"] = "audio/ogg",
            [".pdf"] = "application/pdf",
            [".zip"] = "application/zip",
            [".7z"] = "application/x-7z-compressed",
            [".rar"] = "application/vnd.rar",
        };

        #endregion Private Fields

        #region Public Methods

        public async Task<IFileRequestDataV2> CreateFromStorageFileV2Async(IStorageFile storageFile) {
            FileRequestDataV2 fileRequestData = new FileRequestDataV2();
            var basicProperties = await storageFile.GetBasicPropertiesAsync();
            fileRequestData.FileType = GetContentType(storageFile.FileType);
            fileRequestData.Size = (long)basicProperties.Size;
            fileRequestData.FileName = storageFile.Name;
            fileRequestData.Id = Guid.NewGuid().ToString();
            fileRequestData.Metadata = new FileMetadataV2 {
                Modified = basicProperties.DateModified.ToUniversalTime().ToString("o")
            };
            return fileRequestData;
        }

        public IFileRequestDataV2 CreateFromUniversalSendFileV2(IUniversalSendFileV2 universalSendFile) {
            FileRequestDataV2 fileRequestData = new FileRequestDataV2();
            fileRequestData.Id = universalSendFile.Id;
            fileRequestData.FileName = universalSendFile.FileName;
            fileRequestData.FileType = universalSendFile.FileType;
            fileRequestData.Size = universalSendFile.Size;
            if (!string.IsNullOrEmpty(universalSendFile.Sha256)) {
                fileRequestData.Sha256 = universalSendFile.Sha256;
            }
            if (!string.IsNullOrEmpty(universalSendFile.Preview)) {
                fileRequestData.Preview = universalSendFile.Preview;
            }
            if (universalSendFile.Metadata != null) {
                fileRequestData.Metadata = new FileMetadataV2 {
                    Modified = universalSendFile.Metadata.Modified,
                    Accessed = universalSendFile.Metadata.Accessed
                };
            }
            return fileRequestData;
        }


        #endregion Public Methods

        #region Private Methods

        private static string GetContentType(string fileExtension) {
            if (!string.IsNullOrWhiteSpace(fileExtension) && ContentTypeByExtension.TryGetValue(fileExtension, out string contentType)) {
                return contentType;
            }

            return "application/octet-stream";
        }

        #endregion Private Methods

    }
}