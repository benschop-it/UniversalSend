using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace UniversalSend.Models
{
    public class ViewStorageItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        IStorageItem storageItem = null;
        BitmapImage itemIcon = null;

        protected void OnPropertyChanged([CallerMemberName] string propn = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propn));
        }

        public ViewStorageItem(IStorageItem item)
        {
            storageItem = item;
            _ = GetIconAsync();
        }

        /// <summary>
        /// 文件名
        /// </summary>
        public string Name => storageItem?.Name;
        /// <summary>
        /// 关联的文件
        /// </summary>
        public IStorageItem Item => storageItem;
        /// <summary>
        /// 图标
        /// </summary>
        public BitmapImage Icon
        {
            get { return itemIcon; }
            private set
            {
                if (value != itemIcon)
                {
                    itemIcon = value;
                    OnPropertyChanged();
                }
            }
        }

        private async Task GetIconAsync()
        {
            IRandomAccessStream stream;
            if(storageItem is StorageFile)
            {
                stream = await ((StorageFile)storageItem).GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.SingleItem);
            }else if(storageItem is StorageFolder)
            {
                stream = await ((StorageFolder)storageItem).GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.SingleItem);
            }
            else
            {
                return;
            }

                Icon = new BitmapImage();
            Icon.DecodePixelWidth = 100;
            await Icon.SetSourceAsync(stream);
            stream.Dispose();
        }
    }
}
