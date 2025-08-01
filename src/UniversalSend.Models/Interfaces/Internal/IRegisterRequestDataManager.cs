using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces.Internal {
    internal interface IRegisterRequestDataManager {
        IRegisterRequestData CreateFromDevice(IDevice device);
    }
}