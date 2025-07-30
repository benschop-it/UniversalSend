using System.Collections.Generic;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {
    public interface IHistoryManager {
        List<IHistory> HistoriesList { get; set; }

        void AddHistoriesList(IHistory history);
        void InitHistoriesList();
        void SaveHistoriesList();
        IHistory CreateHistory(IUniversalSendFile file, string futureAccessListToken, IDevice device);
    }
}