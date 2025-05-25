using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Controls.ContentDialogControls;
using UniversalSend.Controls.SendPage;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Tasks;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UniversalSend.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SendPage : Page
    {
        bool Inited = false;

        bool ShareActivated = false;

        public SendPage()
        {
            this.InitializeComponent();
            Register.NewDeviceRegister += Register_NewDeviceRegister;
            DeviceManager.KnownDevicesChanged += DeviceManager_KnownDevicesChanged;
            SendManager.SendCreated += SendManager_SendCreated;
            RootGrid.Margin = UIManager.RootElementMargin;
            SearchFavoriteDevicesAsync();
        }

        private async void SendManager_SendCreated(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                UpdateView();
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter == null)
                return;
            if (e.Parameter is string)
            {
                if(e.Parameter.ToString() == "ShareActivated")
                {
                    ShareActivated = true;
                    SendManager.SendPrepared += SendManager_SendPrepared;
                }
            }
            //if(e.Parameter is List<SendTask>)
            //{
            //    List<SendTask>sendTasks = ((List<SendTask>)e.Parameter);
            //    SendTaskManager.SendTasks.AddRange(sendTasks);
            //    SendManager.SendCreatedEvent();
            //}


        }

        private async void SendManager_SendPrepared(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Frame.Navigate(typeof(FileSendingPage),sender);
            });
        }

        private async void DeviceManager_KnownDevicesChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("刷新已知设备列表");
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Task.Delay(1000);
                KnownDeviceListView.ItemsSource = null;
                KnownDeviceListView.ItemsSource = DeviceManager.KnownDevices;
            });
        }

        private async void Register_NewDeviceRegister(object sender, EventArgs e)
        {
            Debug.WriteLine("刷新已知设备列表");
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                
                Task.Delay(1000);
                KnownDeviceListView.ItemsSource = null;
                KnownDeviceListView.ItemsSource = DeviceManager.KnownDevices;
            });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            KnownDeviceListView.ItemsSource = null;
            KnownDeviceListView.ItemsSource = DeviceManager.KnownDevices;
            if (Inited == false)
            {
                InitButton();
            }
            Inited = true; 
            UpdateView();
        }

        void InitButton()
        {
            SendItemButtonControl MediaButton = new SendItemButtonControl("\uEB9F", "媒体");
            SendItemButtonControl TextButton = new SendItemButtonControl("\uEA37", "文本");
            TextButton.RootButton.Click += TextButton_Click;
            SendItemButtonControl ClipboardContentButton = new SendItemButtonControl("\uF0E3", "剪贴板");
            SendItemButtonControl FileButton = new SendItemButtonControl("\uE7C3", "文件");
            FileButton.RootButton.Click += FileButton_Click;
            SendItemButtonControl FolderButton = new SendItemButtonControl("\uE8B7", "文件夹");
            FolderButton.RootButton.Click += FolderButton_Click;

            SendItemButtonControl AddMediaButton = new SendItemButtonControl("\uEB9F", "媒体");
            SendItemButtonControl AddTextButton = new SendItemButtonControl("\uEA37", "文本");
            AddTextButton.RootButton.Click += TextButton_Click;
            SendItemButtonControl AddClipboardContentButton = new SendItemButtonControl("\uF0E3", "剪贴板");
            SendItemButtonControl AddFileButton = new SendItemButtonControl("\uE7C3", "文件");
            AddFileButton.RootButton.Click += FileButton_Click;
            SendItemButtonControl AddFolderButton = new SendItemButtonControl("\uE8B7", "文件夹");
            AddFolderButton.RootButton.Click += FolderButton_Click;

            /*To-Do:制作媒体选择器*/
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

        

        void AddItemToSendQueue(SendTask task)
        {
            SendTaskManager.SendTasks.Add(task);
            UpdateView();
        }

        void UpdateView()
        {
            if(SendTaskManager.SendTasks.Count!=0)
            {
                SelectSendItemButtons.Visibility = Visibility.Collapsed;
                SendQueueStackPanel.Visibility = Visibility.Visible;
                long totalSize = 0;
                SendQueueItemsStackpanel.Children.Clear();
                foreach (var item in SendTaskManager.SendTasks)
                {
                    SendQueueItemsStackpanel.Children.Add(new Border { Background = new SolidColorBrush { Color = Colors.DarkGray},Height = 45,Width = 45,Margin = new Thickness(2,2,2,2)});
                    totalSize += item.File.Size;
                }
                FileCountTextBlock.Text = $"{LocalizeManager.GetLocalizedString("SendPage_FileCount")}{SendTaskManager.SendTasks.Count}";
                FileSizeTextBlock.Text = $"{LocalizeManager.GetLocalizedString("SendPage_FileSize")}{StringHelper.GetByteUnit(totalSize)}";
            }
            else
            {
                SelectSendItemButtons.Visibility = Visibility.Visible;
                SendQueueStackPanel.Visibility = Visibility.Collapsed;
                SendQueueItemsStackpanel.Children.Clear();
            }
        }

        private async void FileButton_Click(object sender, RoutedEventArgs e)
        {
            await OpenFileAsync();
        }

        async Task OpenFileAsync()
        {
            ProcessProgressBar.Visibility = Visibility.Visible;
            SelectSendItemButtons.IsEnabled = false;
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            var filesReadonlyList = await picker.PickMultipleFilesAsync();
            if (filesReadonlyList != null)
            {
                List<StorageFile>files = filesReadonlyList.ToList();
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        AddItemToSendQueue(await SendTaskManager.CreateSendTask(file));
                    }
                    else
                    {

                    }
                }
            }
            SelectSendItemButtons.IsEnabled = true;
            ProcessProgressBar.Visibility = Visibility.Collapsed;

        }

        public void TextButton_Click(object sender, RoutedEventArgs e)
        {
            TypeTextAsync();
        }

        async Task TypeTextAsync()
        {
            CreateTextSendTaskControl control = new CreateTextSendTaskControl();
            control.CancelButton.Click += (sender, e) =>
            {
                ContentDialogManager.HideContentDialog();
            };
            control.ConfirmButton.Click += (sender, e) =>
            {
                if(!String.IsNullOrEmpty(control.MainTextBox.Text))
                {
                    AddItemToSendQueue(SendTaskManager.CreateSendTask(control.MainTextBox.Text));
                }
                ContentDialogManager.HideContentDialog();
            };
            await ContentDialogManager.ShowContentDialogAsync(control);
        }

        private async void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            await OpenFolderAsync();
        }

        async Task OpenFolderAsync()
        {
            ProcessProgressBar.Visibility = Visibility.Visible;
            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            StorageFolder folder = await picker.PickSingleFolderAsync();
            if(folder!=null)
            {
                List<StorageFile> files = await StorageHelper.GetFilesInFolder(folder);
                
                foreach(StorageFile file in files)
                {
                    AddItemToSendQueue(await SendTaskManager.CreateSendTask(file));
                }
            }
            ProcessProgressBar.Visibility = Visibility.Collapsed;

        }

        private async void KnownDeviceListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(SendTaskManager.SendTasks.Count == 0)
            {
                await MessageDialogManager.EmptySendTaskAsync();
                return;
            }
            Device device = (Device)e.ClickedItem;
            SendManager.SendPreparedEvent(device);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SendTaskManager.SendTasks.Clear();
            SendQueueStackPanel.Visibility = Visibility.Collapsed;
            SelectSendItemButtons.Visibility = Visibility.Visible;
        }

        private async void ManualSendButton_Click(object sender, RoutedEventArgs e)
        {
            if (SendTaskManager.SendTasks.Count == 0)
            {
                await MessageDialogManager.EmptySendTaskAsync();
                return;
            }
            await ManualSendAsync();
        }

        async Task ManualSendAsync()
        {
            ManualSendControl manualSendControl = new ManualSendControl();
            manualSendControl.ConfirmButton.Click += async (sender, e) =>
            {
                manualSendControl.ConfirmButton.IsEnabled = false;
                if (manualSendControl.Mode == 1)
                {
                    if (StringHelper.IsIpaddr(manualSendControl.IPAddressTextBox.Text))
                    {
                        Device device = await DeviceManager.FindDeviceByIPAsync(manualSendControl.IPAddressTextBox.Text);
                        if (device != null)
                        {
                            SendManager.SendPreparedEvent(device);
                            ContentDialogManager.HideContentDialog();
                        }
                        manualSendControl.ErrorMessageTextBlock.Text = $"错误：获取{manualSendControl.IPAddressTextBox}:53317主机信息失败";
                    }
                    manualSendControl.ErrorMessageTextBlock.Text = $"错误：请输入正确的IP地址";
                }
                else
                {
                    manualSendControl.ErrorMessageTextBlock.Text = $"正在寻找与标签对应的主机……";
                    manualSendControl.LoadingProgressBar.Visibility = Visibility.Visible;
                    if (manualSendControl.HashTagTextBox.Text.All(char.IsDigit))
                    {
                        int hashTag = Convert.ToInt32(manualSendControl.HashTagTextBox.Text);
                        Device device = await DeviceManager.FindDeviceByHashTagAsync(manualSendControl.HashTagTextBox.Text);
                        manualSendControl.LoadingProgressBar.Visibility = Visibility.Collapsed;
                        if (device != null)
                        {
                            SendManager.SendPreparedEvent(device);
                            ContentDialogManager.HideContentDialog();
                        }
                        manualSendControl.ErrorMessageTextBlock.Text = $"错误：没有找到与标签对应的主机";
                    }
                    manualSendControl.ErrorMessageTextBlock.Text = $"错误：没有找到与标签对应的主机";
                }
                manualSendControl.ConfirmButton.IsEnabled = true;
            };


            manualSendControl.CancelButton.Click += (sender, e) =>
            {
                ContentDialogManager.HideContentDialog();
            };
            await ContentDialogManager.ShowContentDialogAsync(manualSendControl);
        }

        private async void SearchDevicesButton_Click(object sender, RoutedEventArgs e)
        {
            await SearchDevicesAsync();
        }

        async Task SearchFavoriteDevicesAsync()
        {
            DeviceManager.KnownDevices.Clear();
            SearchDevicesButtonIcon.Visibility = Visibility.Collapsed;
            SearchDevicesButtonProgressRing.Visibility = Visibility.Visible;
            List<string> ipList = new List<string>();
            foreach (var favorite in FavoriteManager.Favorites)
            {
                ipList.Add(favorite.IPAddr);
            }
            await DeviceManager.SearchKnownDevicesAsync(ipList);//优先搜索收藏夹设备
            SearchDevicesButtonIcon.Visibility = Visibility.Visible;
            SearchDevicesButtonProgressRing.Visibility = Visibility.Collapsed;
            KnownDeviceListView.ItemsSource = DeviceManager.KnownDevices;
            SearchDevicesButtonIcon.Visibility = Visibility.Visible;
        }

        async Task SearchDevicesAsync()
        {
            DeviceManager.KnownDevices.Clear();
            

            await SearchFavoriteDevicesAsync();

            SearchDevicesButtonIcon.Visibility = Visibility.Collapsed;
            SearchDevicesButtonProgressRing.Visibility = Visibility.Visible;
            await DeviceManager.SearchKnownDevicesAsync();
            KnownDeviceListView.ItemsSource = null;
            KnownDeviceListView.ItemsSource = DeviceManager.KnownDevices;
            SearchDevicesButtonIcon.Visibility = Visibility.Visible;
            SearchDevicesButtonProgressRing.Visibility = Visibility.Collapsed;
        }

        private async void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            await ContentDialogManager.ShowContentDialogAsync(new FavoritesControl());
        }

        private async void KnownDeviceItemFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            Device device = ((Button)sender).DataContext as Device;
            await ContentDialogManager.ShowContentDialogAsync(new EditFavoriteItemControl(device));
        }
    }
}
