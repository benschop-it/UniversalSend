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
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UniversalSend.Views {

    public sealed partial class SendPage : Page {

        #region Private Fields

        private bool Inited = false;

        private bool ShareActivated = false;

        private ISendTaskManager _sendTaskManager => App.Services.GetRequiredService<ISendTaskManager>();
        private IDeviceManager _deviceManager => App.Services.GetRequiredService<IDeviceManager>();
        private ISendManager _sendManager => App.Services.GetRequiredService<ISendManager>();
        private IContentDialogManager _contentDialogManager => App.Services.GetRequiredService<IContentDialogManager>();
        private IFavoriteManager _favoriteManager => App.Services.GetRequiredService<IFavoriteManager>();
        private IStorageHelper _storageHelper => App.Services.GetRequiredService<IStorageHelper>();
        private IRegister _register => App.Services.GetRequiredService<IRegister>();
        private IUIManager _uiManager => App.Services.GetRequiredService<IUIManager>();

        #endregion Private Fields

        #region Public Constructors

        public SendPage() {
            this.InitializeComponent();
            _register.NewDeviceRegister += Register_NewDeviceRegister;
            _deviceManager.KnownDevicesChanged += DeviceManager_KnownDevicesChanged;
            _sendManager.SendCreated += SendManager_SendCreated;
            RootGrid.Margin = _uiManager.RootElementMargin;
            SearchFavoriteDevicesAsync();
        }

        #endregion Public Constructors

        #region Public Methods

        public void TextButton_Click(object sender, RoutedEventArgs e) {
            TypeTextAsync();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            if (e.Parameter == null) {
                return;
            }

            if (e.Parameter is string) {
                if (e.Parameter.ToString() == "ShareActivated") {
                    ShareActivated = true;
                    _sendManager.SendPrepared += SendManager_SendPrepared;
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddItemToSendQueue(ISendTask task) {
            _sendTaskManager.SendTasks.Add(task);
            UpdateView();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            _sendTaskManager.SendTasks.Clear();
            SendQueueStackPanel.Visibility = Visibility.Collapsed;
            SelectSendItemButtons.Visibility = Visibility.Visible;
        }

        private async void DeviceManager_KnownDevicesChanged(object sender, EventArgs e) {
            Debug.WriteLine("Refresh known device list");
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Task.Delay(1000);
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

        private void InitButton() {
            SendItemButtonControl MediaButton = new SendItemButtonControl("\uEB9F", "Media");
            SendItemButtonControl TextButton = new SendItemButtonControl("\uEA37", "Text");
            TextButton.RootButton.Click += TextButton_Click;
            SendItemButtonControl ClipboardContentButton = new SendItemButtonControl("\uF0E3", "Clipboard");
            SendItemButtonControl FileButton = new SendItemButtonControl("\uE7C3", "File");
            FileButton.RootButton.Click += FileButton_Click;
            SendItemButtonControl FolderButton = new SendItemButtonControl("\uE8B7", "Folder");
            FolderButton.RootButton.Click += FolderButton_Click;

            SendItemButtonControl AddMediaButton = new SendItemButtonControl("\uEB9F", "Media");
            SendItemButtonControl AddTextButton = new SendItemButtonControl("\uEA37", "Text");
            AddTextButton.RootButton.Click += TextButton_Click;
            SendItemButtonControl AddClipboardContentButton = new SendItemButtonControl("\uF0E3", "Clipboard");
            SendItemButtonControl AddFileButton = new SendItemButtonControl("\uE7C3", "File");
            AddFileButton.RootButton.Click += FileButton_Click;
            SendItemButtonControl AddFolderButton = new SendItemButtonControl("\uE8B7", "Folder");
            AddFolderButton.RootButton.Click += FolderButton_Click;

            // To-Do: create media selector
            //SelectSendItemButtonsStackPanel.Children.Add(MediaButton);
            SelectSendItemButtonsStackPanel.Children.Add(TextButton);
            //SelectSendItemButtonsStackPanel.Children.Add(ClipboardContentButton);
            SelectSendItemButtonsStackPanel.Children.Add(FileButton);
            SelectSendItemButtonsStackPanel.Children.Add(FolderButton);

            //AddFlyoutVariableSizedWrapGrid.Children.Add(AddMediaButton);
            AddFlyoutVariableSizedWrapGrid.Children.Add(AddTextButton);
            //AddFlyoutVariableSizedWrapGrid.Children.Add(AddClipboardContentButton);
            AddFlyoutVariableSizedWrapGrid.Children.Add(AddFileButton);
            AddFlyoutVariableSizedWrapGrid.Children.Add(AddFolderButton);
        }

        private async void KnownDeviceItemFavoriteButton_Click(object sender, RoutedEventArgs e) {
            IDevice device = ((Button)sender).DataContext as IDevice;
            await _contentDialogManager.ShowContentDialogAsync(new EditFavoriteItemControl(device));
        }

        private async void KnownDeviceListView_ItemClick(object sender, ItemClickEventArgs e) {
            if (_sendTaskManager.SendTasks.Count == 0) {
                await MessageDialogManager.EmptySendTaskAsync();
                return;
            }
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
            if (_sendTaskManager.SendTasks.Count == 0) {
                await MessageDialogManager.EmptySendTaskAsync();
                return;
            }
            await ManualSendAsync();
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
                        AddItemToSendQueue(await _sendTaskManager.CreateSendTask(file));
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
                    AddItemToSendQueue(await _sendTaskManager.CreateSendTask(file));
                }
            }
            ProcessProgressBar.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            KnownDeviceListView.ItemsSource = null;
            KnownDeviceListView.ItemsSource = _deviceManager.KnownDevices;
            if (!Inited) {
                InitButton();
            }
            Inited = true;
            UpdateView();
            Debug.WriteLine("SendPageLoaded");
        }

        private async void Register_NewDeviceRegister(object sender, EventArgs e) {
            Debug.WriteLine("Refresh known device list");
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Task.Delay(1000);
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
            await SearchDevicesAsync();
        }

        private async Task SearchFavoriteDevicesAsync() {
            _deviceManager.KnownDevices.Clear();
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
                _contentDialogManager.HideContentDialogAsync();
            };
            control.ConfirmButton.Click += (sender, e) => {
                if (!string.IsNullOrEmpty(control.MainTextBox.Text)) {
                    AddItemToSendQueue(_sendTaskManager.CreateSendTask(control.MainTextBox.Text));
                }
                _contentDialogManager.HideContentDialogAsync();
            };
            await _contentDialogManager.ShowContentDialogAsync(control);
        }

        private void UpdateView() {
            if (_sendTaskManager.SendTasks.Count != 0) {
                SelectSendItemButtons.Visibility = Visibility.Collapsed;
                SendQueueStackPanel.Visibility = Visibility.Visible;
                long totalSize = 0;
                SendQueueItemsStackpanel.Children.Clear();
                foreach (var item in _sendTaskManager.SendTasks) {
                    SendQueueItemsStackpanel.Children.Add(new Border { Background = new SolidColorBrush { Color = Colors.DarkGray }, Height = 45, Width = 45, Margin = new Thickness(2) });
                    totalSize += item.File.Size;
                }
                FileCountTextBlock.Text = $"{LocalizeManager.GetLocalizedString("SendPage_FileCount")}{_sendTaskManager.SendTasks.Count}";
                FileSizeTextBlock.Text = $"{LocalizeManager.GetLocalizedString("SendPage_FileSize")}{StringHelper.GetByteUnit(totalSize)}";
            } else {
                SelectSendItemButtons.Visibility = Visibility.Visible;
                SendQueueStackPanel.Visibility = Visibility.Collapsed;
                SendQueueItemsStackpanel.Children.Clear();
            }
        }

        #endregion Private Methods
    }
}