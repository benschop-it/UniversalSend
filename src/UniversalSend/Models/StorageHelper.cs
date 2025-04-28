using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;

namespace UniversalSend.Models
{
    public class StorageHelper
    {
        public static async Task<StorageFile> CreateFileInAppLocalFolderAsync(string fileName)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            return await CreateFileAsync(folder, fileName);
        }

        public static async Task<StorageFile> CreateFileAsync(StorageFolder storageFolder, string fileName)
        {
            if (!await IsItemExsitAsync(storageFolder, fileName))
            {
                return await storageFolder.CreateFileAsync(fileName);
            }
            return null;
        }

        public static async Task WriteBytesToFileAsync(StorageFile file, byte[] data)
        {
            if (file != null && data != null)
            {
                await FileIO.WriteBytesAsync(file, data);
            }
        }

        public static async Task<bool> IsItemExsitAsync(StorageFolder parentFolder, string itemName)
        {

            IStorageItem storageItem = await parentFolder.TryGetItemAsync(itemName);
            if (storageItem == null)
                return false;
            else
                return true;
        }

        public static async void LaunchAppLocalFolder()
        {
            var t = new FolderLauncherOptions();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            await Launcher.LaunchFolderAsync(folder, t);
        }

        public static async Task WriteTestFileAsync(byte[] content)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await storageFolder.GetFileAsync("test");
            await WriteBytesToFileAsync(storageFile,content);
        }
    }
}
