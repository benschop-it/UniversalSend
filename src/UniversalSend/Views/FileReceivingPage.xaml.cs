using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalSend.Models;
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
    public sealed partial class FileReceivingPage : Page
    {
        public FileReceivingPage()
        {
            this.InitializeComponent();
            //ReceiveTaskManager.ReceivingTasks.Clear();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //ReceiveTask receiveTask = new ReceiveTask();
            //receiveTask.file = new Models.Data.UniversalSendFile();
            //receiveTask.file.FileName = "test";
            //receiveTask.TaskState = ReceiveTask.ReceiveTaskStates.Wating;
            //FileReceivingListView.Items.Add(receiveTask);
            //receiveTask = new ReceiveTask();
            //receiveTask.file = new Models.Data.UniversalSendFile();
            //receiveTask.file.FileName = "test";
            //receiveTask.TaskState = ReceiveTask.ReceiveTaskStates.Error;
            //FileReceivingListView.Items.Add(receiveTask);
            ReceiveManager.SendDataReceived += ReceiveManager_SendDataReceived;
            ReceiveManager.CancelReceived += ReceiveManager_CancelReceived;
            FileReceivingListView.ItemsSource = ReceiveTaskManager.ReceivingTasks;
            MainProgressBar.Maximum = ReceiveTaskManager.ReceivingTasks.Count;
        }

        private async void ReceiveManager_CancelReceived(object sender, EventArgs e)
        {
            /*To-Do:取消操作*/
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                PageHeader.Text = "传输已取消";
                ProgressBarLabel.Text = "已取消";
                MainProgressBar.ShowError = true;
            });
            
        }

        int ReceivedItemsCount = 0;

        private async void ReceiveManager_SendDataReceived(object sender, EventArgs e)
        {
            ReceivedItemsCount++;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                UpdateUI();
            });
            
        }

        public void UpdateUI()
        {
            FileReceivingListView.ItemsSource = null;
            FileReceivingListView.ItemsSource = ReceiveTaskManager.ReceivingTasks;
            if (ReceivedItemsCount == ReceiveTaskManager.ReceivingTasks.Count)
            {
                ProgressBarLabel.Text = "已完成";
                FinishButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Collapsed;
                
            }
            MainProgressBar.Value = ReceivedItemsCount;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            //To-Do:加入历史记录
            ReceiveTaskManager.ReceivingTasks.Clear();
            Frame.GoBack();
        }
    }
}
