using System.Collections.Generic;

namespace UniversalSend.Models.Interfaces {
    public interface IHistoryManager {

        #region Public Properties

        List<IHistory> HistoriesList { get; set; }

        #endregion Public Properties

        #region Public Methods

        void AddHistoriesList(IHistory history);
        IHistory CreateHistory(IUniversalSendFile file, string futureAccessListToken, IDevice device);

        void InitHistoriesList();
        void SaveHistoriesList();

        #endregion Public Methods
    }
}