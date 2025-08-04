using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniversalSend.Misc;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using UniversalSend.Services.Interfaces;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
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

            //Debug.WriteLine($"UpdateUI: _receivedItemsCount: {_receivedItemsCount}, tasksCount: {tasksCount}");

            int unfinishedTasks = 0;
            bool done = true;
            foreach (var task in _receiveTaskManager.ReceivingTasks) {
                //Debug.Write($"{task.Progress} {task.Status} {task.TaskState} {task.File.FileName} ");

                if (
                    task.TaskState == ReceiveTaskStates.Waiting ||
                    task.TaskState == ReceiveTaskStates.Sending ||
                    task.TaskState == ReceiveTaskStates.Receiving 
                ) {
                    done = false;
                    unfinishedTasks++;
                    //Debug.Write($"-> UNFINISHED {unfinishedTasks}");
                } else {
                    //Debug.Write("-> DONE");
                }
                //Debug.WriteLine("");
            }

            if (done) {
                ProgressBarLabel.Text = "Completed";
                FinishButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            var progress = 100.0 - 100.0 * (double)unfinishedTasks / (double)tasksCount;
            //Debug.WriteLine($"Progress = {progress}");

            MainProgressBar.Value = progress;
        }

        #endregion Public Methods

        #region Private Methods

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e) {
            //To-Do: Add to history
            _receiveTaskManager.ReceivingTasks.Clear();
            Frame.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            _receiveManager.SendDataReceived += ReceiveManager_SendDataReceived;
            _receiveManager.CancelReceived += ReceiveManager_CancelReceived;
            _receiveManager.SendRequestProgressReceived += ReceiveManager_SendRequestProgressReceived;
            UpdateUI();
            //MainProgressBar.Maximum = _receiveTaskManager.ReceivingTasks.Count;
        }

        private async void ReceiveManager_SendRequestProgressReceived(object sender, EventArgs args) {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                UpdateUI();
            });

            //// Prefer strongly-typed EventArgs; if not available, your cast is fine:
            //if (!(sender is ISendRequestProgress e)) return;

            //var receivingTasks = _receiveTaskManager.ReceivingTasks;

            //IReceiveTask currentTask = null;

            //Dictionary<string, string> parameters = StringHelper.GetURLQueryParameters(e.Uri.ToString());
            //if (parameters.Count > 0) {
            //    if (parameters.ContainsKey("fileId") && parameters.ContainsKey("token")) {
            //        var fileId = parameters["fileId"];
            //        var token = parameters["token"];

            //        foreach (IReceiveTask task in receivingTasks) {
            //            var file = task.File;
            //            if (file.Id == fileId && file.TransferToken == token) {
            //                currentTask = task;
            //                break;
            //            }
            //        }
            //    }
            //}
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
                UpdateUI();
            });
        }

        #endregion Private Methods
    }
}