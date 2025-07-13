using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Controls.ItemControls;
using UniversalSend.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// For more information on the "UserControl" item template, see https://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalSend.Controls.ContentDialogControls
{
    public sealed partial class StorageItemPropertiesControl : UserControl
    {
        IStorageItem StorageItem { get; set; }
        ViewStorageItem ViewStorageItem { get; set; }

        public StorageItemPropertiesControl(IStorageItem storageItem)
        {
            this.InitializeComponent();
            StorageItem = storageItem;
            ViewStorageItem = new ViewStorageItem(storageItem);
            // ViewStorageItem.PropertyChanged += ViewStorageItem_PropertyChanged;
        }

        // private async void ViewStorageItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        // {
        //     await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        //     {
        //     });
        // }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _ = UpdateViewAsync();
        }

        async Task UpdateViewAsync()
        {
            NameTextBlock.Text = StorageItem.Name;
            BasicProperties properties = await StorageItem.GetBasicPropertiesAsync();

            PropertiesStackPanel.Children.Add(new StorageItemPropertyItemControl("Location", StorageItem.Path.Substring(0, StorageItem.Path.LastIndexOf("\\"))));
            PropertiesStackPanel.Children.Add(new StorageItemPropertyItemControl("Size", StringHelper.GetByteUnit((long)properties.Size)));
            PropertiesStackPanel.Children.Add(new StorageItemPropertyItemControl("Created", properties.ItemDate.ToString()));
            PropertiesStackPanel.Children.Add(new StorageItemPropertyItemControl("Modified", properties.DateModified.ToString()));
        }
    }
}
