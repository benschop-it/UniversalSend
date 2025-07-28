using System;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Views {

    public sealed partial class FileReceivingPage : Page {

        #region Private Fields

        private int _receivedItemsCount = 0;

        #endregion Private Fields

        #region Public Constructors

        public FileReceivingPage() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void UpdateUI() {
            FileReceivingListView.ItemsSource = null;
            FileReceivingListView.ItemsSource = ReceiveTaskManager.ReceivingTasks;
            if (_receivedItemsCount == ReceiveTaskManager.ReceivingTasks.Count) {
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
            ReceiveTaskManager.ReceivingTasks.Clear();
            Frame.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            ReceiveManager.SendDataReceived += ReceiveManager_SendDataReceived;
            ReceiveManager.CancelReceived += ReceiveManager_CancelReceived;
            UpdateUI();
            MainProgressBar.Maximum = ReceiveTaskManager.ReceivingTasks.Count;
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