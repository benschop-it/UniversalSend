using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UniversalSend.Controls.ContentDialogControls;
using UniversalSend.Controls.SendPage;
using UniversalSend.Interfaces;
using UniversalSend.Misc;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Interfaces;
using Windows.Storage;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace UniversalSend.Views {

    public sealed partial class SendPage : Page {

        #region Private Fields

        private bool Inited = false;

        private ISendTaskManager _sendTaskManager => App.Services.GetRequiredService<ISendTaskManager>();
        private IDeviceManager _deviceManager => App.Services.GetRequiredService<IDeviceManager>();
        private ISendManager _sendManager => App.Services.GetRequiredService<ISendManager>();
        private IContentDialogManager _contentDialogManager => App.Services.GetRequiredService<IContentDialogManager>();
        private IFavoriteManager _favoriteManager => App.Services.GetRequiredService<IFavoriteManager>();
        private IStorageHelper _storageHelper => App.Services.GetRequiredService<IStorageHelper>();
        private IRegister _register => App.Services.GetRequiredService<IRegister>();
        private IUIManager _uiManager => App.Services.GetRequiredService<IUIManager>();
        private IUdpDiscoveryService _udpDiscoveryService => App.Services.GetRequiredService<IUdpDiscoveryService>();

        #endregion Private Fields

        #region Public Constructors

        public SendPage() {
            this.InitializeComponent();
            RootGrid.Margin = _uiManager.RootElementMargin;
        }

        #endregion Public Constructors

        #region Public Methods

        public async void TextButton_Click(object sender, RoutedEventArgs e) {
            await TypeTextAsync();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            _register.NewDeviceRegister -= Register_NewDeviceRegister;
            _deviceManager.KnownDevicesChanged -= DeviceManager_KnownDevicesChanged;
            _sendManager.SendCreated -= SendManager_SendCreated;
            _sendManager.SendPrepared -= SendManager_SendPrepared;

            _register.NewDeviceRegister += Register_NewDeviceRegister;
            _deviceManager.KnownDevicesChanged += DeviceManager_KnownDevicesChanged;
            _sendManager.SendCreated += SendManager_SendCreated;

            if (e.Parameter == null) {
                return;
            }

            if (e.Parameter is string) {
                if (e.Parameter.ToString() == "ShareActivated") {
                    _sendManager.SendPrepared += SendManager_SendPrepared;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            _register.NewDeviceRegister -= Register_NewDeviceRegister;
            _deviceManager.KnownDevicesChanged -= DeviceManager_KnownDevicesChanged;
            _sendManager.SendCreated -= SendManager_SendCreated;
            _sendManager.SendPrepared -= SendManager_SendPrepared;
            base.OnNavigatedFrom(e);
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddItemToSendQueue(ISendTaskV2 task) {
            _sendTaskManager.ClearWebShare();
            _sendTaskManager.SendTasksV2.Add(task);
            UpdateView();
        }

        private void ReplaceClipboardItemInSendQueue(ISendTaskV2 task) {
            _sendTaskManager.ClearWebShare();

            for (int i = _sendTaskManager.SendTasksV2.Count - 1; i >= 0; i--) {
                if (IsClipboardTask(_sendTaskManager.SendTasksV2[i])) {
                    _sendTaskManager.SendTasksV2.RemoveAt(i);
                }
            }

            _sendTaskManager.SendTasksV2.Add(task);
            UpdateView();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            _sendTaskManager.ClearWebShare();
            _sendTaskManager.SendTasksV2.Clear();
            SendQueueStackPanel.Visibility = Visibility.Collapsed;
            SelectSendItemButtons.Visibility = Visibility.Visible;
        }

        private async void DeviceManager_KnownDevicesChanged(object sender, EventArgs e) {
            await Task.Delay(1000);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                KnownDeviceListView.ItemsSource = null;
                KnownDeviceListView.ItemsSource = _deviceManager.KnownDevices;
            });
        }

        private async void FavoriteButton_Click(object sender, RoutedEventArgs e) {
            await _contentDialogManager.ShowContentDialogAsync(new FavoritesControl());
        }

        private async void FileButton_Click(object sender, RoutedEventArgs e) {
            await OpenFileAsync();
        }

        private async void FolderButton_Click(object sender, RoutedEventArgs e) {
            await OpenFolderAsync();
        }

        private async void PasteButton_Click(object sender, RoutedEventArgs e) {
            DataPackageView clipboard = Clipboard.GetContent();
            bool containsImage = clipboard.Contains(StandardDataFormats.StorageItems) || clipboard.Contains(StandardDataFormats.Bitmap);

            if (clipboard.Contains(StandardDataFormats.StorageItems)) {
                try {
                    IReadOnlyList<IStorageItem> storageItems = await clipboard.GetStorageItemsAsync();
                    StorageFile imageFile = storageItems?
                        .OfType<StorageFile>()
                        .FirstOrDefault(x => x.ContentType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true);

                    if (imageFile != null) {
                        ReplaceClipboardItemInSendQueue(await _sendTaskManager.CreateSendTaskV2(imageFile));
                        return;
                    }
                } catch (Exception ex) {
                    Debug.WriteLine($"Paste storage item failed: {ex}");
                }
            }

            if (clipboard.Contains(StandardDataFormats.Bitmap)) {
                try {
                    RandomAccessStreamReference bitmapRef = await clipboard.GetBitmapAsync();
                    if (bitmapRef != null) {
                        using (IRandomAccessStreamWithContentType stream = await bitmapRef.OpenReadAsync()) {
                            if (stream == null || stream.Size == 0) {
                                return;
                            }

                            var reader = new DataReader(stream.GetInputStreamAt(0));
                            await reader.LoadAsync((uint)stream.Size);
                            byte[] bytes = new byte[(int)stream.Size];
                            reader.ReadBytes(bytes);

                            StorageFile tempFile = await _storageHelper.CreateTempFile($"Clipboard-{DateTime.Now:yyyyMMdd-HHmmss-fff}.png");
                            await _storageHelper.WriteBytesToFileAsync(tempFile, bytes);
                            ReplaceClipboardItemInSendQueue(await _sendTaskManager.CreateSendTaskV2(tempFile));
                            return;
                        }
                    }
                } catch (Exception ex) {
                    Debug.WriteLine($"Paste bitmap failed: {ex}");
                }

                await MessageDialogManager.ShowMessageAsync("Clipboard image data could not be read.", "Paste");
                return;
            }

            if (!containsImage && clipboard.Contains(StandardDataFormats.Text)) {
                string text = await clipboard.GetTextAsync();
                if (!string.IsNullOrWhiteSpace(text)) {
                    ReplaceClipboardItemInSendQueue(_sendTaskManager.CreateSendTaskV2(text));
                    return;
                }
            }

            await MessageDialogManager.ShowMessageAsync("Clipboard does not contain text or an image.", "Paste");
        }

        private void InitButton() {
            SendItemButtonControl MediaButton = new SendItemButtonControl("\uEB9F", "Media");
            SendItemButtonControl TextButton = new SendItemButtonControl("\uEA37", "Text");
            TextButton.RootButton.Click += TextButton_Click;
            SendItemButtonControl ClipboardContentButton = new SendItemButtonControl("\uF0E3", "Paste");
            ClipboardContentButton.RootButton.Click += PasteButton_Click;
            SendItemButtonControl FileButton = new SendItemButtonControl("\uE7C3", "File");
            FileButton.RootButton.Click += FileButton_Click;
            SendItemButtonControl FolderButton = new SendItemButtonControl("\uE8B7", "Folder");
            FolderButton.RootButton.Click += FolderButton_Click;

            SendItemButtonControl AddMediaButton = new SendItemButtonControl("\uEB9F", "Media");
            SendItemButtonControl AddTextButton = new SendItemButtonControl("\uEA37", "Text");
            AddTextButton.RootButton.Click += TextButton_Click;
            SendItemButtonControl AddClipboardContentButton = new SendItemButtonControl("\uF0E3", "Paste");
            AddClipboardContentButton.RootButton.Click += PasteButton_Click;
            SendItemButtonControl AddFileButton = new SendItemButtonControl("\uE7C3", "File");
            AddFileButton.RootButton.Click += FileButton_Click;
            SendItemButtonControl AddFolderButton = new SendItemButtonControl("\uE8B7", "Folder");
            AddFolderButton.RootButton.Click += FolderButton_Click;

            // To-Do: create media selector
            //SelectSendItemButtonsStackPanel.Children.Add(MediaButton);
            SelectSendItemButtonsStackPanel.Children.Add(FileButton);
            SelectSendItemButtonsStackPanel.Children.Add(ClipboardContentButton);
            SelectSendItemButtonsStackPanel.Children.Add(TextButton);
            SelectSendItemButtonsStackPanel.Children.Add(FolderButton);

            //AddFlyoutVariableSizedWrapGrid.Children.Add(AddMediaButton);
            AddFlyoutVariableSizedWrapGrid.Children.Add(AddFileButton);
            AddFlyoutVariableSizedWrapGrid.Children.Add(AddClipboardContentButton);
            AddFlyoutVariableSizedWrapGrid.Children.Add(AddTextButton);
            AddFlyoutVariableSizedWrapGrid.Children.Add(AddFolderButton);
        }

        private async void KnownDeviceItemFavoriteButton_Click(object sender, RoutedEventArgs e) {
            IDevice device = ((Button)sender).DataContext as IDevice;
            await _contentDialogManager.ShowContentDialogAsync(new EditFavoriteItemControl(device));
        }

        private async void KnownDeviceListView_ItemClick(object sender, ItemClickEventArgs e) {
            if (_sendTaskManager.SendTasksV2.Count == 0) {
                await MessageDialogManager.EmptySendTaskAsync();
                return;
            }

            _sendTaskManager.ClearWebShare();
            IDevice device = (IDevice)e.ClickedItem;
            _sendManager.SendPreparedEvent(device);
        }

        private async Task ManualSendAsync() {
            ManualSendControl manualSendControl = new ManualSendControl();
            manualSendControl.ConfirmButton.Click += async (sender, e) => {
                manualSendControl.ConfirmButton.IsEnabled = false;
                if (manualSendControl.Mode == 1) {
                    if (StringHelper.IsIpaddr(manualSendControl.IPAddressTextBox.Text)) {
                        IDevice device = await _deviceManager.FindDeviceByIPAsync(manualSendControl.IPAddressTextBox.Text);
                        if (device != null) {
                            _sendManager.SendPreparedEvent(device);
                            await _contentDialogManager.HideContentDialogAsync();
                        }
                        manualSendControl.ErrorMessageTextBlock.Text = $"Error: failed to get host info for {manualSendControl.IPAddressTextBox.Text}:53317";
                    }
                    manualSendControl.ErrorMessageTextBlock.Text = $"Error: please enter a valid IP address";
                } else {
                    manualSendControl.ErrorMessageTextBlock.Text = $"Searching for host matching the hashtag...";
                    manualSendControl.LoadingProgressBar.Visibility = Visibility.Visible;
                    if (manualSendControl.HashTagTextBox.Text.All(char.IsDigit)) {
                        int hashTag = Convert.ToInt32(manualSendControl.HashTagTextBox.Text);
                        IDevice device = await _deviceManager.FindDeviceByHashTagAsync(manualSendControl.HashTagTextBox.Text);
                        manualSendControl.LoadingProgressBar.Visibility = Visibility.Collapsed;
                        if (device != null) {
                            _sendManager.SendPreparedEvent(device);
                            await _contentDialogManager.HideContentDialogAsync();
                        }
                        manualSendControl.ErrorMessageTextBlock.Text = $"Error: no host found for this hashtag";
                    }
                    manualSendControl.ErrorMessageTextBlock.Text = $"Error: no host found for this hashtag";
                }
                manualSendControl.ConfirmButton.IsEnabled = true;
            };

            manualSendControl.CancelButton.Click += (sender, e) => {
                _contentDialogManager.HideContentDialogAsync();
            };
            await _contentDialogManager.ShowContentDialogAsync(manualSendControl);
        }

        private async void ManualSendButton_Click(object sender, RoutedEventArgs e) {
            if (_sendTaskManager.SendTasksV2.Count == 0) {
                await MessageDialogManager.EmptySendTaskAsync();
                return;
            }
            await ManualSendAsync();
        }

        private async void WebShareButton_Click(object sender, RoutedEventArgs e) {
            if (_sendTaskManager.SendTasksV2.Count == 0) {
                await MessageDialogManager.EmptySendTaskAsync();
                return;
            }

            _sendTaskManager.PublishForWebShare();

            if (string.IsNullOrWhiteSpace(_sendTaskManager.LastWebShareUrl)) {
                await MessageDialogManager.ShowMessageAsync(
                    string.IsNullOrWhiteSpace(_sendTaskManager.LastWebShareErrorMessage)
                        ? "Unable to create a browser download share."
                        : _sendTaskManager.LastWebShareErrorMessage,
                    "Web Share");
                return;
            }

            var webShareControl = new WebShareControl();
            webShareControl.StopSharing += (s, args) => {
                _contentDialogManager.HideContentDialogAsync();
            };
            await _contentDialogManager.ShowContentDialogAsync(webShareControl);
        }

        private async Task OpenFileAsync() {
            ProcessProgressBar.Visibility = Visibility.Visible;
            SelectSendItemButtons.IsEnabled = false;
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            var filesReadonlyList = await picker.PickMultipleFilesAsync();
            if (filesReadonlyList != null) {
                List<StorageFile> files = filesReadonlyList.ToList();
                foreach (var file in files) {
                    if (file != null) {
                        AddItemToSendQueue(await _sendTaskManager.CreateSendTaskV2(file));
                    }
                }
            }
            SelectSendItemButtons.IsEnabled = true;
            ProcessProgressBar.Visibility = Visibility.Collapsed;
        }

        private async Task OpenFolderAsync() {
            ProcessProgressBar.Visibility = Visibility.Visible;
            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            StorageFolder folder = await picker.PickSingleFolderAsync();
            if (folder != null) {
                List<StorageFile> files = await _storageHelper.GetFilesInFolder(folder);
                foreach (StorageFile file in files) {
                    AddItemToSendQueue(await _sendTaskManager.CreateSendTaskV2(file));
                }
            }
            ProcessProgressBar.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            KnownDeviceListView.ItemsSource = null;
            KnownDeviceListView.ItemsSource = _deviceManager.KnownDevices;
            if (!Inited) {
                InitButton();
                _ = SearchFavoriteDevicesAsync();
            }
            Inited = true;
            UpdateView();
        }

        private async void Register_NewDeviceRegister(object sender, EventArgs e) {
            await Task.Delay(1000);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                KnownDeviceListView.ItemsSource = null;
                KnownDeviceListView.ItemsSource = _deviceManager.KnownDevices;
            });
        }

        private async Task SearchDevicesAsync() {
            _deviceManager.KnownDevices.Clear();

            await SearchFavoriteDevicesAsync();
            SearchDevicesButtonIcon.Visibility = Visibility.Collapsed;
            SearchDevicesButtonProgressRing.Visibility = Visibility.Visible;

            KnownDeviceListView.ItemsSource = null;
            KnownDeviceListView.ItemsSource = _deviceManager.KnownDevices;
            SearchDevicesButtonIcon.Visibility = Visibility.Visible;
            SearchDevicesButtonProgressRing.Visibility = Visibility.Collapsed;
        }

        private async void SearchDevicesButton_Click(object sender, RoutedEventArgs e) {
            await _udpDiscoveryService.SendAnnouncementAsyncV2();
            //await SearchDevicesAsync();
        }

        private async Task SearchFavoriteDevicesAsync() {
            //_deviceManager.KnownDevices.Clear();
            SearchDevicesButtonIcon.Visibility = Visibility.Collapsed;
            SearchDevicesButtonProgressRing.Visibility = Visibility.Visible;

            List<string> ipList = new List<string>();
            foreach (var favorite in _favoriteManager.Favorites) {
                ipList.Add(favorite.IPAddr);
            }

            await _deviceManager.SearchKnownDevicesAsync(ipList); // Priority search for favorites
            SearchDevicesButtonIcon.Visibility = Visibility.Visible;
            SearchDevicesButtonProgressRing.Visibility = Visibility.Collapsed;
            KnownDeviceListView.ItemsSource = _deviceManager.KnownDevices;
        }

        private async void SendManager_SendCreated(object sender, EventArgs e) {
            await Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    UpdateView();
                });
        }

        private async void SendManager_SendPrepared(object sender, EventArgs e) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Frame.Navigate(typeof(FileSendingPage), sender);
            });
        }

        private async Task TypeTextAsync() {
            CreateTextSendTaskControl control = new CreateTextSendTaskControl();
            control.CancelButton.Click += (sender, e) => {
                _ = _contentDialogManager.HideContentDialogAsync();
            };
            control.ConfirmButton.Click += (sender, e) => {
                if (!string.IsNullOrEmpty(control.MainTextBox.Text)) {
                    AddItemToSendQueue(_sendTaskManager.CreateSendTaskV2(control.MainTextBox.Text));
                }
                _ = _contentDialogManager.HideContentDialogAsync();
            };
            await _contentDialogManager.ShowContentDialogAsync(control);
        }

        private void UpdateView() {
            if (_sendTaskManager.SendTasksV2.Count != 0) {
                SelectSendItemButtons.Visibility = Visibility.Collapsed;
                SendQueueStackPanel.Visibility = Visibility.Visible;
                long totalSize = 0;
                SendQueueItemsStackpanel.Children.Clear();
                foreach (var item in _sendTaskManager.SendTasksV2) {
                    SendQueueItemsStackpanel.Children.Add(CreateSendQueueThumbnail(item));
                    totalSize += item.File.Size;
                }
                FileCountTextBlock.Text = $"{LocalizeManager.GetLocalizedString("SendPage_FileCount")}{_sendTaskManager.SendTasksV2.Count}";
                FileSizeTextBlock.Text = $"{LocalizeManager.GetLocalizedString("SendPage_FileSize")}{StringHelper.GetByteUnit(totalSize)}";
            } else {
                SelectSendItemButtons.Visibility = Visibility.Visible;
                SendQueueStackPanel.Visibility = Visibility.Collapsed;
                SendQueueItemsStackpanel.Children.Clear();
            }
        }

        private FrameworkElement CreateSendQueueThumbnail(ISendTaskV2 task) {
            var root = new StackPanel {
                Width = 72,
                Margin = new Thickness(2),
            };

            var border = new Border {
                Width = 64,
                Height = 64,
                Background = new SolidColorBrush(Colors.DimGray),
                CornerRadius = new CornerRadius(4),
                HorizontalAlignment = HorizontalAlignment.Center,
                Child = CreateThumbnailContent(task),
            };

            ToolTipService.SetToolTip(border, task?.File?.FileName ?? string.Empty);
            root.Children.Add(border);

            root.Children.Add(new TextBlock {
                Text = task?.File?.FileName ?? string.Empty,
                TextWrapping = TextWrapping.NoWrap,
                TextTrimming = TextTrimming.CharacterEllipsis,
                FontSize = 11,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextAlignment = TextAlignment.Center,
            });

            return root;
        }

        private FrameworkElement CreateThumbnailContent(ISendTaskV2 task) {
            if (task?.StorageFile != null && IsImageTask(task)) {
                var image = new Image {
                    Stretch = Stretch.UniformToFill,
                };
                _ = LoadThumbnailAsync(task.StorageFile, image);
                return image;
            }

            return new Viewbox {
                Child = new FontIcon {
                    Glyph = GetQueueIconGlyph(task),
                    Foreground = new SolidColorBrush(Colors.White),
                }
            };
        }

        private async Task LoadThumbnailAsync(IStorageFile file, Image image) {
            try {
                var bitmap = new BitmapImage();

                if (file is StorageFile storageFile) {
                    using (var thumb = await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, 96)) {
                        if (thumb == null) {
                            return;
                        }

                        await bitmap.SetSourceAsync(thumb);
                    }
                } else {
                    using (var stream = await file.OpenReadAsync()) {
                        await bitmap.SetSourceAsync(stream);
                    }
                }

                image.Source = bitmap;
            } catch (Exception ex) {
                Debug.WriteLine($"Thumbnail load failed for '{file?.Path}': {ex}");
            }
        }

        private static bool IsImageTask(ISendTaskV2 task) {
            return task?.File != null &&
                   !string.IsNullOrWhiteSpace(task.File.FileType) &&
                   task.File.FileType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetQueueIconGlyph(ISendTaskV2 task) {
            if (task?.File == null) {
                return "\uE7C3";
            }

            if (!string.IsNullOrWhiteSpace(task.File.FileType)) {
                if (task.File.FileType.StartsWith("text/", StringComparison.OrdinalIgnoreCase)) {
                    return "\uEA37";
                }

                if (task.File.FileType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)) {
                    return "\uEB9F";
                }
            }

            return "\uE7C3";
        }

        private static bool IsClipboardTask(ISendTaskV2 task) {
            if (task?.File == null) {
                return false;
            }

            if (task.StorageFile != null) {
                return task.File.FileName?.StartsWith("Clipboard-", StringComparison.OrdinalIgnoreCase) == true;
            }

            return string.Equals(task.File.FileType, "text/plain", StringComparison.OrdinalIgnoreCase);
        }

        #endregion Private Methods
    }
}