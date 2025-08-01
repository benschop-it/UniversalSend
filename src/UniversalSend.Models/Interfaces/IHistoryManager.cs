using System.Collections.Generic;

namespace UniversalSend.Models.Interfaces {
    public interface IHistoryManager {
        List<IHistory> HistoriesList { get; set; }

        void AddHistoriesList(IHistory history);
        void InitHistoriesList();
        void SaveHistoriesList();
        IHistory CreateHistory(IUniversalSendFile file, string futureAccessListToken, IDevice device);
    }
}