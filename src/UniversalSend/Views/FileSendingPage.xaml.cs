using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Controls;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class FileSendingPage : Page
    {
        Device Device { get; set; }

        public FileSendingPage()
        {
            this.InitializeComponent();
            SendManager.SendStateChanged += SendManager_SendStateChanged;
            MainProgressBar.Maximum = SendTaskManager.SendTasks.Count;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Device = (Device)e.Parameter;
            await StartTaskAsync();

        }

        async Task StartTaskAsync()
        {
            LocalDeviceGrid.Children.Add(new DeviceItemControl(ProgramData.LocalDevice));
            ReceiveDeviceGrid.Children.Add(new DeviceItemControl(Device));
            bool sendSendRequestSuccess = await SendTaskManager.SendSendRequestAsync(Device);
            if(sendSendRequestSuccess == false)
            {
                PrepareLabel.Text = "接收方拒绝了传输请求。";
                return;
            }
            PrepareControls.Visibility = Visibility.Collapsed;
            SendingControls.Visibility = Visibility.Visible;
            await SendTaskManager.SendSendTasksAsync(Device);
        }

        private async void SendManager_SendStateChanged(object sender, EventArgs e)
        {
            SendedItemsCount++;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                UpdateUI();
            });
        }

        int SendedItemsCount = 0;

        void UpdateUI()
        {
            FileSendingListView.ItemsSource = null;
            FileSendingListView.ItemsSource = SendTaskManager.SendTasks;
            if (SendedItemsCount == SendTaskManager.SendTasks.Count)
            {
                ProgressBarLabel.Text = "已完成";
                FinishButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Collapsed;

            }
            MainProgressBar.Value = SendedItemsCount;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            SendTaskManager.SendTasks.Clear();
            Frame.GoBack();
        }
    }
}
