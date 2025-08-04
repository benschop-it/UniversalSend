using System;
using System.Threading.Tasks;
using UniversalSend.Services.Interfaces;

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

        event EventHandler SendRequestProgressReceived;

        #endregion Public Events

        #region Public Properties

        bool? ChosenOption { get; set; }
        QuickSaveMode QuickSave { get; set; }

        #endregion Public Properties

        #region Public Methods

        void CancelReceivedEvent();

        Task<bool> GetChosenOption();

        void SendDataReceivedEvent(IReceiveTask receiveTask);

        void SendProgressEvent(ISendRequestProgress sendRequestProgress);

        void SendRequestEvent(ISendRequestData sendRequestData);

        #endregion Public Methods
    }
}