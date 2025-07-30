using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversalSend.Models.Interfaces {
    #region Public Enums

    public enum DeviceType {
        mobile,
        desktop,
        web,
        headless,
        server
    }

    #endregion Public Enums

    public interface IDeviceManager {
        List<IDevice> KnownDevices { get; set; }

        event EventHandler KnownDevicesChanged;

        void AddKnownDevices(IDevice device);
        void ClearKnownDevices();
        IDevice CreateDeviceFromInfoData(IInfoData info);
        Task<IDevice> FindDeviceByHashTagAsync(string HashTag);
        Task<IDevice> FindDeviceByIPAsync(string IP);
        Task SearchKnownDevicesAsync(List<string> ipList);

        IDevice GetDeviceFromRequestData(IRegisterRequestData requestData, string ip, int port);

        IDevice GetDeviceFromResponseData(IRegisterResponseData responseData, string ip);

        IDevice CreateDevice(string alias, string ip, int port);

        IDevice GetLocalDevice();
    }
}