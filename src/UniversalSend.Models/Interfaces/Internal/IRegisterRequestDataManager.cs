using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces.Internal {
    internal interface IRegisterRequestDataManager {

        #region Public Methods

        IRegisterRequestDataV1 CreateFromDevice(IDevice device);

        #endregion Public Methods
    }
}