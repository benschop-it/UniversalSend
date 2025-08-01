using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {
    public interface IStorageHelper {

        #region Public Methods

        Task<StorageFile> CreateFileAsync(StorageFolder storageFolder, string fileName);
        Task<StorageFile> CreateFileInAppLocalFolderAsync(string fileName);
        Task<StorageFile> CreateFileInDownloadsFolderAsync(string fileName);
        Task<StorageFolder> CreateFolderInAppLocalFolderAsync(string folderName);
        Task<StorageFile> CreateTempFile(string fileName);
        Task<List<StorageFile>> GetFilesInFolder(StorageFolder folder);
        Task<StorageFolder> GetReceiveStorageFolderAsync();
        Task<bool> IsItemExsitAsync(StorageFolder parentFolder, string itemName);
        void LaunchAppLocalFolder();
        Task<byte[]> ReadBytesFromFileAsync(IStorageFile file);
        Task WriteBytesToFileAsync(IStorageFile file, byte[] data);
        Task WriteTestFileAsync(byte[] content);

        #endregion Public Methods
    }
}