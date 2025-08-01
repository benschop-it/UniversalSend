namespace UniversalSend.Models.Interfaces {

    public enum DeviceFormFactorType {
        Phone,
        Desktop,
        Tablet,
        IoT,
        SurfaceHub,
        Xbox,
        Holographic,
        Other
    }

    public interface ISystemHelper {

        #region Public Methods

        DeviceFormFactorType GetDeviceFormFactorType();

        #endregion Public Methods
    }
}