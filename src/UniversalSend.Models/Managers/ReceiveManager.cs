using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;

namespace UniversalSend.Models.Managers {

    public class ReceiveManager {

        #region Public Enums

        public enum QuickSaveMode {
            Off,
            Favorites,
            On,
        }

        #endregion Public Enums

        #region Public Events

        // Event triggered upon receiving a Cancel event
        public static event EventHandler CancelReceived;

        // Event triggered upon receiving actual Send data
        public static event EventHandler SendDataReceived;

        // Event triggered upon receiving a Send-Request
        public static event EventHandler SendRequestReceived;

        #endregion Public Events

        #region Public Properties

        public static bool? ChosenOption { get; set; }

        public static QuickSaveMode QuickSave { get; set; } = QuickSaveMode.Off;

        #endregion Public Properties

        #region Public Methods

        public static void CancelReceivedEvent() {
            Debug.WriteLine("CancelReceivedEvent");
            ReceiveTaskManager.ReceivingTasks.Clear();
            CancelReceived?.Invoke(null, EventArgs.Empty);
        }

        public static async Task<bool> GetChosenOption() {
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

        public static void SendDataReceivedEvent(ReceiveTask receiveTask) {
            Debug.WriteLine("SendDataReceivedEvent");
            SendDataReceived?.Invoke(receiveTask, EventArgs.Empty);
        }

        public static void SendRequestEvent(SendRequestData sendRequestData) {
            Debug.WriteLine("SendRequestEvent");
            SendRequestReceived?.Invoke(sendRequestData, EventArgs.Empty);
        }

        #endregion Public Methods
    }
}