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

// https://go.microsoft.com/fwlink/?LinkId=234238 Describes the "Blank Page" item template

namespace UniversalSend.Views
{
    /// <summary>
    /// Can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FileSendingPage : Page
    {
        Device Device { get; set; }

        public FileSendingPage()
        {
            this.InitializeComponent();
            SendManager.SendStateChanged += SendManager_SendStateChanged;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Device = (Device)e.Parameter;
            UpdateUI();
            MainProgressBar.Maximum = SendTaskManager.SendTasks.Count;
            await StartTaskAsync();
        }

        async Task StartTaskAsync()
        {
            LocalDeviceGrid.Children.Add(new DeviceItemControl(ProgramData.LocalDevice));
            ReceiveDeviceGrid.Children.Add(new DeviceItemControl(Device));
            bool sendSendRequestSuccess = await SendTaskManager.SendSendRequestAsync(Device);
            if (sendSendRequestSuccess == false)
            {
                PrepareLabel.Text = "The receiver declined the transfer request.";
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
                ProgressBarLabel.Text = "Completed";
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
