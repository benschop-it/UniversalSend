using System;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;

namespace UniversalSend.Models.Interfaces {
    #region Public Enums

    public enum QuickSaveMode {
        Off,
        Favorites,
        On,
    }

    #endregion Public Enums
    public interface IReceiveManager {
        bool? ChosenOption { get; set; }
        QuickSaveMode QuickSave { get; set; }

        event EventHandler CancelReceived;
        event EventHandler SendDataReceived;
        event EventHandler SendRequestReceived;

        void CancelReceivedEvent();
        Task<bool> GetChosenOption();
        void SendDataReceivedEvent(IReceiveTask receiveTask);
        void SendRequestEvent(SendRequestData sendRequestData);
    }
}