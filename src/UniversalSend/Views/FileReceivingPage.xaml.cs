using Microsoft.Extensions.DependencyInjection;
using System;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
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
        }

        #endregion Public Constructors

        #region Public Methods

        public void UpdateUI() {
            FileReceivingListView.ItemsSource = null;
            FileReceivingListView.ItemsSource = _receiveTaskManager.ReceivingTasks;
            if (_receivedItemsCount == _receiveTaskManager.ReceivingTasks.Count) {
                ProgressBarLabel.Text = "Completed";
                FinishButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            MainProgressBar.Value = _receivedItemsCount;
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
            UpdateUI();
            MainProgressBar.Maximum = _receiveTaskManager.ReceivingTasks.Count;
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