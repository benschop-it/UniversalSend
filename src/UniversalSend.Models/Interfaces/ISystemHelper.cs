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
        DeviceFormFactorType GetDeviceFormFactorType();
    }
}