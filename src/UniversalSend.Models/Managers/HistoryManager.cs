using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;
using Windows.Storage.AccessCache;

namespace UniversalSend.Models.Managers {

    internal class HistoryManager : IHistoryManager {

        private ISettings _settings;

        #region Public Properties

        public List<IHistory> HistoriesList { get; set; } = new List<IHistory>();

        #endregion Public Properties

        public HistoryManager(ISettings settings) {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #region Public Methods

        public void AddHistoriesList(IHistory history) {
            HistoriesList.Add(history);
            SaveHistoriesList();
        }

        public IHistory CreateHistory(IUniversalSendFile file, string futureAccessListToken, IDevice device) {
            var history = new History(file, futureAccessListToken, device);
            return history;
        }

        public void InitHistoriesList() {
            string str = _settings.GetSettingContentAsString(Constants.Receive_Histories);
            if (String.IsNullOrEmpty(str)) {
                return;
            }

            List<History> list = JsonConvert.DeserializeObject<List<History>>(str);
            if (list != null) {
                HistoriesList = list?.Cast<IHistory>().ToList();
            }
        }

        public void SaveHistoriesList() {
            _settings.SetSetting(Constants.Receive_Histories, JsonConvert.SerializeObject(HistoriesList));
        }

        #endregion Public Methods
    }
}