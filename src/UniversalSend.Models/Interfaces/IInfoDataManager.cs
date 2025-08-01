using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces {
    public interface IInfoDataManager {
        InfoData GetInfoDataFromDevice();
    }
}