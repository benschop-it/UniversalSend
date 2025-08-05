using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using UniversalSend.Controls;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UniversalSend.Views {

    public sealed partial class FileSendingPage : Page {

        #region Private Fields

        private int _sendedItemsCount = 0;
        private ISendManager _sendManager => App.Services.GetRequiredService<ISendManager>();
        private ISendTaskManager _sendTaskManager => App.Services.GetRequiredService<ISendTaskManager>();
        private IDeviceManager _deviceManager => App.Services.GetRequiredService<IDeviceManager>();

        #endregion Private Fields

        #region Public Constructors

        public FileSendingPage() {
            InitializeComponent();
            _sendManager.SendStateChanged += SendManager_SendStateChanged;
        }

        #endregion Public Constructors

        #region Private Properties

        private IDevice Device { get; set; }

        #endregion Private Properties

        #region Protected Methods

        protected override async void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            Device = (IDevice)e.Parameter;
            UpdateUI();
            MainProgressBar.Maximum = _sendTaskManager.SendTasksV2.Count;
            await StartTaskAsync();
        }

        #endregion Protected Methods

        #region Private Methods

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            Frame.GoBack();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e) {
            _sendTaskManager.SendTasksV2.Clear();
            Frame.GoBack();
        }

        private async void SendManager_SendStateChanged(object sender, EventArgs e) {
            _sendedItemsCount++;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                UpdateUI();
            });
        }

        private async Task StartTaskAsync() {
            LocalDeviceGrid.Children.Add(new DeviceItemControl(_deviceManager.GetLocalDevice()));
            ReceiveDeviceGrid.Children.Add(new DeviceItemControl(Device));
            //bool sendSendRequestSuccess = await _sendTaskManager.SendSendRequestV1Async(Device);
            bool sendSendRequestSuccess = await _sendTaskManager.SendSendRequestV2Async(Device);
            if (sendSendRequestSuccess == false) {
                PrepareLabel.Text = "The receiver declined the transfer request.";
                return;
            }
            PrepareControls.Visibility = Visibility.Collapsed;
            SendingControls.Visibility = Visibility.Visible;
            await _sendTaskManager.SendSendTasksV2Async(Device);
        }

        private void UpdateUI() {
            FileSendingListView.ItemsSource = null;
            FileSendingListView.ItemsSource = _sendTaskManager.SendTasksV2;
            if (_sendedItemsCount == _sendTaskManager.SendTasksV2.Count) {
                ProgressBarLabel.Text = "Completed";
                FinishButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            MainProgressBar.Value = _sendedItemsCount;
        }

        #endregion Private Methods
    }
}