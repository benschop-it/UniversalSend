using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    internal class ReceiveManager : IReceiveManager {

        private readonly IReceiveTaskManager _receiveTaskManager;

        public ReceiveManager(IReceiveTaskManager receiveTaskManager) {
            _receiveTaskManager = receiveTaskManager ?? throw new ArgumentNullException(nameof(receiveTaskManager));
        }

        #region Public Events

        // Event triggered upon receiving a Cancel event
        public event EventHandler CancelReceived;

        // Event triggered upon receiving actual Send data
        public event EventHandler SendDataReceived;

        // Event triggered upon receiving a Send-Request
        public event EventHandler SendRequestReceived;

        #endregion Public Events

        #region Public Properties

        public bool? ChosenOption { get; set; }

        public QuickSaveMode QuickSave { get; set; } = QuickSaveMode.Off;

        #endregion Public Properties

        #region Public Methods

        public void CancelReceivedEvent() {
            Debug.WriteLine("CancelReceivedEvent");
            _receiveTaskManager.ReceivingTasks.Clear();
            CancelReceived?.Invoke(null, EventArgs.Empty);
        }

        public async Task<bool> GetChosenOption() {
            await Task.Run(async () => {
                int waitTime = 0;
                while (ChosenOption == null) {
                    await Task.Delay(500);
                    if (waitTime++ > 100) {
                        ChosenOption = false;
                        return;
                    }
                }
            });
            bool option = (bool)ChosenOption;
            ChosenOption = null;
            return option;
        }

        public void SendDataReceivedEvent(IReceiveTask receiveTask) {
            Debug.WriteLine("SendDataReceivedEvent");
            SendDataReceived?.Invoke(receiveTask, EventArgs.Empty);
        }

        public void SendRequestEvent(ISendRequestData sendRequestData) {
            Debug.WriteLine("SendRequestEvent");
            SendRequestReceived?.Invoke(sendRequestData, EventArgs.Empty);
        }

        #endregion Public Methods
    }
}