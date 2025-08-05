using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces {
    public interface IInfoDataManager {

        #region Public Methods

        InfoDataV1 GetInfoDataV1FromDevice();

        InfoDataV2 GetInfoDataV2FromDevice();

        #endregion Public Methods
    }
}