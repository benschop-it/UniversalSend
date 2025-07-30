using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using Windows.System;

namespace UniversalSend.Models.Helpers {

    internal class StorageHelper : IStorageHelper {

        private readonly ISystemHelper _systemHelper;
        private readonly ISettings _settings;

        public StorageHelper(ISystemHelper systemHelper, ISettings settings) {
            _systemHelper = systemHelper ?? throw new ArgumentNullException(nameof(systemHelper));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #region Public Methods

        public async Task<StorageFile> CreateFileAsync(StorageFolder storageFolder, string fileName) {
            if (!await IsItemExsitAsync(storageFolder, fileName)) {
                return await storageFolder.CreateFileAsync(fileName);
            } else {
                string fileType = fileName.Substring(fileName.Length - fileName.LastIndexOf("."));
                for (int i = 1; i < 999; i++) {
                    if (!await IsItemExsitAsync(storageFolder, fileType + $"({i})" + fileType)) {
                        return await storageFolder.CreateFileAsync(fileType + $"({i})" + fileType);
                    }
                }
            }
            return null;
        }

        public async Task<StorageFile> CreateFileInAppLocalFolderAsync(string fileName) {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            return await CreateFileAsync(folder, fileName);
        }

        public async Task<StorageFile> CreateFileInDownloadsFolderAsync(string fileName) {
            return await DownloadsFolder.CreateFileAsync(fileName);
        }

        public async Task<StorageFolder> CreateFolderInAppLocalFolderAsync(string folderName) {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            IStorageItem storageItem = await folder.GetItemAsync(folderName);
            if (storageItem == null || storageItem is StorageFile) {
                return await folder.CreateFolderAsync(folderName);
            } else {
                return await folder.GetFolderAsync(folderName);
            }
        }

        public async Task<StorageFile> CreateTempFile(string fileName) {
            StorageFolder folder = await CreateFolderInAppLocalFolderAsync("Temp");
            return await CreateFileAsync(folder, fileName);
        }

        //public static StorageFolder DefaultSaveFolder = DownloadsFolder.
        public async Task<List<StorageFile>> GetFilesInFolder(StorageFolder folder) {
            var items = await folder.GetItemsAsync();
            List<StorageFile> files = new List<StorageFile>();
            foreach (var item in items) {
                if (item is StorageFile) {
                    files.Add((StorageFile)item);
                } else if (item is StorageFolder) {
                    files.AddRange(await GetFilesInFolder((StorageFolder)item));
                }
            }
            return files;
        }

        public async Task<StorageFolder> GetReceiveStorageFolderAsync() {
            if (_systemHelper.GetDeviceFormFactorType() == DeviceFormFactorType.Xbox) {
                return ApplicationData.Current.LocalFolder;
            } else {
                string folderToken = _settings.GetSettingContentAsString(Constants.Receive_SaveToFolder);
                if (!String.IsNullOrEmpty(folderToken) && StorageApplicationPermissions.FutureAccessList.ContainsItem(folderToken)) {
                    try {
                        StorageFolder storageFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folderToken);
                        return storageFolder;
                    } catch {
                        return null;
                    }
                } else {
                    return null;
                }
            }
        }

        public async Task<bool> IsItemExsitAsync(StorageFolder parentFolder, string itemName) {
            IStorageItem storageItem = await parentFolder.TryGetItemAsync(itemName);
            return storageItem == null ? false : true;
        }

        public async void LaunchAppLocalFolder() {
            var t = new FolderLauncherOptions();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            await Launcher.LaunchFolderAsync(folder, t);
        }

        public async Task<byte[]> ReadBytesFromFileAsync(IStorageFile file) {
            if (file != null) {
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                byte[] bytes = buffer.ToArray();
                return bytes;
            }
            return null;
        }

        public async Task WriteBytesToFileAsync(IStorageFile file, byte[] data) {
            if (file != null && data != null) {
                await FileIO.WriteBytesAsync(file, data);
            }
        }

        public async Task WriteTestFileAsync(byte[] content) {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.GetFileAsync("test");
            await WriteBytesToFileAsync(storageFile, content);
        }

        #endregion Public Methods
    }
}