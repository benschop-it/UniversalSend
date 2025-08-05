using System;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.Interfaces {
    public interface IRegister {

        #region Public Events

        event EventHandler NewDeviceRegister;

        #endregion Public Events

        #region Public Methods

        void NewDeviceRegisterEvent(IDevice device);
        void StartRegister();

        #endregion Public Methods
    }
}