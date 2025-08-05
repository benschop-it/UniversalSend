using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversalSend.Models.Interfaces {

    public enum DeviceType {
        mobile,
        desktop,
        web,
        headless,
        server
    }

    public interface IDeviceManager {

        #region Public Events

        event EventHandler KnownDevicesChanged;

        #endregion Public Events

        #region Public Properties

        List<IDevice> KnownDevices { get; set; }

        #endregion Public Properties

        #region Public Methods

        void AddKnownDevices(IDevice device);
        void ClearKnownDevices();
        IDevice CreateDevice(string alias, string ip, int port);
        IDevice CreateDeviceFromInfoDataV2(IInfoDataV2 info);
        Task<IDevice> FindDeviceByHashTagAsync(string HashTag);
        Task<IDevice> FindDeviceByIPAsync(string IP);
        IDevice GetDeviceFromRequestDataV2(IRegisterRequestDataV2 requestData, string ip, int port);
        IDevice GetDeviceFromResponseDataV2(IAnnouncementV2 responseData, string ip);
        IDevice GetLocalDevice();
        Task SearchKnownDevicesAsync(List<string> ipList);

        #endregion Public Methods
    }
}