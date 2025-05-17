using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using Windows.Storage;

namespace UniversalSend.Models.HttpData
{
    public class FileRequestData
    {
        public string id { get; set; }
        public string fileName { get; set; }
        public long size { get; set; } // in bytes
        public string fileType { get; set; }
        public string preview { get; set; } // Nullable
    }

    public class FileRequestDataManager
    {
        public static async Task<FileRequestData> CreateFromStorageFileAsync(StorageFile storageFile)
        {
            FileRequestData fileRequestData = new FileRequestData();
            fileRequestData.fileType = storageFile.FileType;
            fileRequestData.size = (long)(await storageFile.GetBasicPropertiesAsync()).Size;
            fileRequestData.fileName = storageFile.Name;
            fileRequestData.id = Guid.NewGuid().ToString();
            return fileRequestData;
                /*await StorageHelper.ReadBytesFromFileAsync(storageFile);*/

        }

        public static FileRequestData CreateFromUniversalSendFile(UniversalSendFile universalSendFile)
        {
            FileRequestData fileRequestData = new FileRequestData();
            fileRequestData.fileType = universalSendFile.FileType;
            fileRequestData.fileName = universalSendFile.FileName;
            fileRequestData.size = universalSendFile.Size;
            fileRequestData.id = universalSendFile.Id;
            if(!String.IsNullOrEmpty(universalSendFile.Text))
            {
                fileRequestData.preview = universalSendFile.Text;
            }
            return fileRequestData;
        }
    }
}
