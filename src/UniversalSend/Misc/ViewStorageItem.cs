using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace UniversalSend.Misc {

    public class ViewStorageItem : INotifyPropertyChanged {

        #region Private Fields

        private BitmapImage _itemIcon = null;

        #endregion Private Fields

        #region Public Constructors

        public ViewStorageItem(IStorageItem item) {
            Item = item;
            _ = GetIconAsync();
        }

        #endregion Public Constructors

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Icon
        /// </summary>
        public BitmapImage Icon {
            get { return _itemIcon; }
            private set {
                if (value != _itemIcon) {
                    _itemIcon = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Associated file
        /// </summary>
        public IStorageItem Item { get; } = null;

        /// <summary>
        /// File name
        /// </summary>
        public string Name => Item?.Name;

        #endregion Public Properties

        #region Protected Methods

        protected void OnPropertyChanged([CallerMemberName] string propn = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propn));
        }

        #endregion Protected Methods

        #region Private Methods

        private async Task GetIconAsync() {
            IRandomAccessStream stream;
            if (Item is StorageFile) {
                stream = await ((StorageFile)Item).GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.SingleItem);
            } else if (Item is StorageFolder) {
                stream = await ((StorageFolder)Item).GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.SingleItem);
            } else {
                return;
            }

            Icon = new BitmapImage();
            Icon.DecodePixelWidth = 100;
            await Icon.SetSourceAsync(stream);
            stream.Dispose();
        }

        #endregion Private Methods
    }
}