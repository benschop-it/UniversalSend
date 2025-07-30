using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UniversalSend.Controls.ItemControls;
using UniversalSend.Misc;
using UniversalSend.Models.Helpers;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Controls.ContentDialogControls {

    public sealed partial class StorageItemPropertiesControl : UserControl {

        #region Public Constructors

        public StorageItemPropertiesControl(IStorageItem storageItem) {
            this.InitializeComponent();
            StorageItem = storageItem;
            ViewStorageItem = new ViewStorageItem(storageItem);
            ViewStorageItem.PropertyChanged += ViewStorageItem_PropertyChanged;
        }

        #endregion Public Constructors

        #region Private Properties

        private IStorageItem StorageItem { get; set; }
        private ViewStorageItem ViewStorageItem { get; set; }

        #endregion Private Properties

        private async void ViewStorageItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Debug.WriteLine("ViewStorageItem_PropertyChanged called.");
            });
        }

        #region Private Methods

        private async Task UpdateViewAsync() {
            NameTextBlock.Text = StorageItem.Name;
            BasicProperties properties = await StorageItem.GetBasicPropertiesAsync();

            PropertiesStackPanel.Children.Add(new StorageItemPropertyItemControl("Location", StorageItem.Path.Substring(0, StorageItem.Path.LastIndexOf("\\"))));
            PropertiesStackPanel.Children.Add(new StorageItemPropertyItemControl("Size", StringHelper.GetByteUnit((long)properties.Size)));
            PropertiesStackPanel.Children.Add(new StorageItemPropertyItemControl("Created", properties.ItemDate.ToString()));
            PropertiesStackPanel.Children.Add(new StorageItemPropertyItemControl("Modified", properties.DateModified.ToString()));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            _ = UpdateViewAsync();
        }

        #endregion Private Methods
    }
}