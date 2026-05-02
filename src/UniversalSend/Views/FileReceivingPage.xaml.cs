using Microsoft.Extensions.DependencyInjection;
using System;
using UniversalSend.Misc;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using UniversalSend.Services.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Views {

    public sealed partial class FileReceivingPage : Page {

        #region Private Fields

        private int _receivedItemsCount = 0;
        private IReceiveTaskManager _receiveTaskManager => App.Services.GetRequiredService<IReceiveTaskManager>();
        private IReceiveManager _receiveManager => App.Services.GetRequiredService<IReceiveManager>();

        #endregion Private Fields

        #region Public Constructors

        public FileReceivingPage() {
            InitializeComponent();
            FileReceivingListView.ItemsSource = _receiveTaskManager.ReceivingTasks;
        }

        #endregion Public Constructors

        #region Public Methods

        public void UpdateUI() {
            int tasksCount = _receiveTaskManager.ReceivingTasks.Count;

            int unfinishedTasks = 0;
            bool done = true;
            foreach (var task in _receiveTaskManager.ReceivingTasks) {
                if (
                    task.TaskState == ReceiveTaskStates.Waiting ||
                    task.TaskState == ReceiveTaskStates.Sending ||
                    task.TaskState == ReceiveTaskStates.Receiving 
                ) {
                    done = false;
                    unfinishedTasks++;
                }
            }

            if (done) {
                ProgressBarLabel.Text = "Completed";
                FinishButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            double progress = 0.0;
            if (tasksCount != 0) {
                progress = 100.0 - 100.0 * (double)unfinishedTasks / (double)tasksCount;
            }

            MainProgressBar.Value = progress;
        }

        #endregion Public Methods

        #region Private Methods

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e) {
            //To-Do: Add to history
            _receiveTaskManager.ClearReceivingTasks();
            Frame.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            _receiveManager.SendDataReceived += ReceiveManager_SendDataReceived;
            _receiveManager.CancelReceived += ReceiveManager_CancelReceived;
            _receiveManager.SendRequestProgressReceived += ReceiveManager_SendRequestProgressReceived;
            UpdateUI();
        }

        private async void ReceiveManager_SendRequestProgressReceived(object sender, EventArgs args) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                UpdateUI();
            });
        }

        private async void ReceiveManager_CancelReceived(object sender, EventArgs e) {
            /*To-Do: Cancel operation */
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                PageHeader.Text = "Transfer canceled";
                ProgressBarLabel.Text = "Canceled";
                MainProgressBar.ShowError = true;
                //TODO: Cancel the operation
            });
        }

        private async void ReceiveManager_SendDataReceived(object sender, EventArgs e) {
            _receivedItemsCount++;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                if (IsTextTask(sender as IReceiveTask)) {
                    Frame.Navigate(typeof(ReceivedTextPage), sender);
                    return;
                }

                UpdateUI();
            });
        }

        private static bool IsTextTask(IReceiveTask task) {
            return task?.FileV2 != null &&
                   !string.IsNullOrWhiteSpace(task.FileV2.FileType) &&
                   task.FileV2.FileType.StartsWith("text/", StringComparison.OrdinalIgnoreCase);
        }

        #endregion Private Methods
    }
}