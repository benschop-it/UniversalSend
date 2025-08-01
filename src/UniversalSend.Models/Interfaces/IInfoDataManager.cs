using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces {
    public interface IInfoDataManager {

        #region Public Methods

        InfoData GetInfoDataFromDevice();

        #endregion Public Methods
    }
}