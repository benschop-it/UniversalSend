using System;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;

namespace UniversalSend.Models.Interfaces {

    public enum QuickSaveMode {
        Off,
        Favorites,
        On,
    }

    public interface IReceiveManager {

        #region Public Events

        event EventHandler CancelReceived;

        event EventHandler SendDataReceived;

        event EventHandler SendRequestReceived;

        #endregion Public Events

        #region Public Properties

        bool? ChosenOption { get; set; }
        QuickSaveMode QuickSave { get; set; }

        #endregion Public Properties

        #region Public Methods

        void CancelReceivedEvent();
        Task<bool> GetChosenOption();
        void SendDataReceivedEvent(IReceiveTask receiveTask);
        void SendRequestEvent(ISendRequestData sendRequestData);

        #endregion Public Methods
    }
}