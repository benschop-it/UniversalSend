using System;
using System.Collections.Generic;
using System.IO;
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

        #region Private Fields

        private readonly ISettings _settings;
        private readonly ISystemHelper _systemHelper;

        #endregion Private Fields

        #region Public Constructors

        public StorageHelper(ISystemHelper systemHelper, ISettings settings) {
            _systemHelper = systemHelper ?? throw new ArgumentNullException(nameof(systemHelper));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<StorageFile> CreateFileAsync(StorageFolder storageFolder, string fileName) {
            if (storageFolder == null || string.IsNullOrWhiteSpace(fileName)) {
                return null;
            }

            var parts = SplitRelativePath(fileName);
            if (parts.Count == 0) {
                return null;
            }

            var leafFileName = parts[parts.Count - 1];
            var targetFolder = await EnsureFolderPathAsync(storageFolder, parts.Take(parts.Count - 1));

            if (!await IsItemExsitAsync(targetFolder, leafFileName)) {
                return await targetFolder.CreateFileAsync(leafFileName);
            }

            var dotIndex = leafFileName.LastIndexOf('.');
            var baseName = dotIndex > 0 ? leafFileName.Substring(0, dotIndex) : leafFileName;
            var extension = dotIndex > 0 ? leafFileName.Substring(dotIndex) : string.Empty;

            for (int i = 1; i < 999; i++) {
                var candidate = $"{baseName}({i}){extension}";
                if (!await IsItemExsitAsync(targetFolder, candidate)) {
                    return await targetFolder.CreateFileAsync(candidate);
                }
            }

            return null;
        }

        public async Task<StorageFile> CreateFileInAppLocalFolderAsync(string fileName) {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            return await CreateFileAsync(folder, fileName);
        }

        public async Task<StorageFile> CreateFileInDownloadsFolderAsync(string fileName) {
            var parts = SplitRelativePath(fileName);
            var leafFileName = parts.Count == 0 ? fileName : parts[parts.Count - 1];
            return await DownloadsFolder.CreateFileAsync(leafFileName, CreationCollisionOption.GenerateUniqueName);
        }

        public async Task<StorageFolder> CreateFolderInAppLocalFolderAsync(string folderName) {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            return await EnsureFolderPathAsync(folder, SplitRelativePath(folderName));
        }

        public async Task<StorageFile> CreateTempFile(string fileName) {
            StorageFolder folder = await CreateFolderInAppLocalFolderAsync("Temp");
            return await CreateFileAsync(folder, fileName);
        }

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
            if (parentFolder == null || string.IsNullOrWhiteSpace(itemName)) {
                return false;
            }

            var parts = SplitRelativePath(itemName);
            if (parts.Count == 0) {
                return false;
            }

            var currentFolder = parentFolder;
            for (int i = 0; i < parts.Count - 1; i++) {
                var folderItem = await currentFolder.TryGetItemAsync(parts[i]);
                if (!(folderItem is StorageFolder nextFolder)) {
                    return false;
                }

                currentFolder = nextFolder;
            }

            IStorageItem storageItem = await currentFolder.TryGetItemAsync(parts[parts.Count - 1]);
            return storageItem == null ? false : true;
        }

        public async void LaunchAppLocalFolder() {
            var t = new FolderLauncherOptions();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            await Launcher.LaunchFolderAsync(folder, t);
        }

        public async Task DeleteFileAsync(IStorageFile file) {
            if (file == null) {
                return;
            }

            try {
                await file.DeleteAsync();
            } catch {
            }
        }

        public async Task MoveFileAsync(IStorageFile sourceFile, IStorageFile destinationFile) {
            if (sourceFile == null || destinationFile == null) {
                return;
            }

            var buffer = await FileIO.ReadBufferAsync(sourceFile);
            await FileIO.WriteBufferAsync(destinationFile, buffer);

            try {
                await sourceFile.DeleteAsync();
            } catch {
            }
        }

        public async Task<byte[]> ReadBytesFromFileAsync(IStorageFile file) {
            if (file != null) {
                IBuffer buffer = await FileIO.ReadBufferAsync(file);
                byte[] bytes = buffer.ToArray();
                return bytes;
            }
            return null;
        }

        public async Task<Stream> OpenReadStreamAsync(IStorageFile file) {
            if (file == null) {
                return null;
            }

            var randomAccessStream = await file.OpenReadAsync();
            return randomAccessStream.AsStreamForRead();
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

        #region Private Methods

        private static List<string> SplitRelativePath(string relativePath) {
            return (relativePath ?? string.Empty)
                .Replace('\\', '/')
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.Equals(x, ".", StringComparison.Ordinal) && !string.Equals(x, "..", StringComparison.Ordinal))
                .ToList();
        }

        private static async Task<StorageFolder> EnsureFolderPathAsync(StorageFolder rootFolder, IEnumerable<string> folderParts) {
            var currentFolder = rootFolder;
            foreach (var part in folderParts) {
                if (string.IsNullOrWhiteSpace(part)) {
                    continue;
                }

                var existing = await currentFolder.TryGetItemAsync(part);
                if (existing is StorageFolder existingFolder) {
                    currentFolder = existingFolder;
                } else {
                    currentFolder = await currentFolder.CreateFolderAsync(part, CreationCollisionOption.OpenIfExists);
                }
            }

            return currentFolder;
        }

        #endregion Private Methods

    }
}