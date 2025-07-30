using System;
using UniversalSend.Models.Data;

namespace UniversalSend.Models.Interfaces {
    public interface IRegister {
        event EventHandler NewDeviceRegister;

        void NewDeviceRegisterV1Event(IDevice device);
        void StartRegister();
    }
}