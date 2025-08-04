using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Misc;
using UniversalSend.Strings;

namespace UniversalSend.Models.Managers {

    internal class HistoryManager : IHistoryManager {

        #region Private Fields

        private ISettings _settings;

        #endregion Private Fields

        #region Public Constructors

        public HistoryManager(ISettings settings) {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #endregion Public Constructors

        #region Public Properties

        public List<IHistory> HistoriesList { get; set; } = new List<IHistory>();

        #endregion Public Properties

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

            List<ConcreteHistory> list = JsonConvert.DeserializeObject<List<ConcreteHistory>>(str);
            if (list != null) {
                foreach (ConcreteHistory conreteHistory in list) {
                    History h = new History(conreteHistory.File, conreteHistory.FutureAccessListToken, conreteHistory.Device);
                    HistoriesList.Add(h);
                }
            }
        }

        private static object _lockObject = new object();

        public void SaveHistoriesList() {
            lock (_lockObject) {
                var histories = new List<IHistory>(HistoriesList);
                _settings.SetSetting(Constants.Receive_Histories, JsonConvert.SerializeObject(histories));
            }
        }

        #endregion Public Methods

    }
}