using System;
using System.Threading.Tasks;
using UniversalSend.Controls;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UniversalSend.Views {

    public sealed partial class FileSendingPage : Page {

        #region Private Fields

        private int _sendedItemsCount = 0;

        #endregion Private Fields

        #region Public Constructors

        public FileSendingPage() {
            InitializeComponent();
            SendManager.SendStateChanged += SendManager_SendStateChanged;
        }

        #endregion Public Constructors

        #region Private Properties

        private Device Device { get; set; }

        #endregion Private Properties

        #region Protected Methods

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            Device = (Device)e.Parameter;
            UpdateUI();
            MainProgressBar.Maximum = SendTaskManager.SendTasks.Count;
            await StartTaskAsync();
        }

        #endregion Protected Methods

        #region Private Methods

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e) {
            SendTaskManager.SendTasks.Clear();
            Frame.GoBack();
        }

        private async void SendManager_SendStateChanged(object sender, EventArgs e) {
            _sendedItemsCount++;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                UpdateUI();
            });
        }

        private async Task StartTaskAsync() {
            LocalDeviceGrid.Children.Add(new DeviceItemControl(ProgramData.LocalDevice));
            ReceiveDeviceGrid.Children.Add(new DeviceItemControl(Device));
            bool sendSendRequestSuccess = await SendTaskManager.SendSendRequestAsync(Device);
            if (sendSendRequestSuccess == false) {
                PrepareLabel.Text = "The receiver declined the transfer request.";
                return;
            }
            PrepareControls.Visibility = Visibility.Collapsed;
            SendingControls.Visibility = Visibility.Visible;
            await SendTaskManager.SendSendTasksAsync(Device);
        }

        private void UpdateUI() {
            FileSendingListView.ItemsSource = null;
            FileSendingListView.ItemsSource = SendTaskManager.SendTasks;
            if (_sendedItemsCount == SendTaskManager.SendTasks.Count) {
                ProgressBarLabel.Text = "Completed";
                FinishButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            MainProgressBar.Value = _sendedItemsCount;
        }

        #endregion Private Methods
    }
}