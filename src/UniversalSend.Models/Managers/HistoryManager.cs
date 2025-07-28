using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UniversalSend.Models.Managers {

    public class HistoryManager {

        #region Public Properties

        public static List<History> HistoriesList { get; set; } = new List<History>();

        #endregion Public Properties

        #region Public Methods

        public static void AddHistoriesList(History history) {
            HistoriesList.Add(history);
            SaveHistoriesList();
        }

        public static void InitHistoriesList() {
            string str = Settings.GetSettingContentAsString(Settings.Receive_Histories);
            if (String.IsNullOrEmpty(str)) {
                return;
            }

            List<History> list = JsonConvert.DeserializeObject<List<History>>(str);
            if (list != null) {
                HistoriesList = list;
            }
        }

        public static void SaveHistoriesList() {
            Settings.SetSetting(Settings.Receive_Histories, JsonConvert.SerializeObject(HistoriesList));
        }

        #endregion Public Methods
    }
}