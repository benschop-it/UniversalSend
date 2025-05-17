using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;

namespace UniversalSend.Models
{
    public class History
    {
        public History(UniversalSendFile file, string futureAccessListToken, Device device)
        {
            this.File = file;
            this.FutureAccessListToken = futureAccessListToken;
            this.Device = device;
        }

        public UniversalSendFile File { get; set; }

        public string FutureAccessListToken { get; set; }

        public Device Device { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;
    }
    public class HistoryManager
    {
        public static List<History> HistoriesList { get; set; } = new List<History>();

        public static void InitHistoriesList()
        {
            string str = Settings.GetSettingContentAsString(Settings.Receive_Histories);
            if (String.IsNullOrEmpty(str))
                return;
            List<History>list = JsonConvert.DeserializeObject<List<History>>(str);
            if(list != null)
            {
                HistoriesList = list;
            }
        }

        public static void AddHistoriesList(History history)
        {
            HistoriesList.Add(history);
            SaveHistoriesList();
        }

        public static void SaveHistoriesList()
        {
            Settings.SetSetting(Settings.Receive_Histories, JsonConvert.SerializeObject(HistoriesList));
        }
    }
}
