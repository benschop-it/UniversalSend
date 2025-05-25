using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Data;

namespace UniversalSend.Models
{
    public class Favorite
    {
        public Favorite(string deviceName,string ipAddr,long port)
        {
            DeviceName = deviceName;
            IPAddr = ipAddr;
            Port = port;
        }
        public string DeviceName { get; set; }
        public string IPAddr { get; set; }
        public long Port { get; set; }
    }

    public class FavoriteManager
    {
        public static List<Favorite> Favorites { get; set; } = new List<Favorite>();

        public static void InitFavoritesData()
        {
            string str = Settings.GetSettingContentAsString(Settings.Favorite_Favorites);
            if (String.IsNullOrEmpty(str))
                return;
            Favorites = JsonConvert.DeserializeObject<List<Favorite>>(str);
        }

        public static void SaveFavoritesData()
        {
            Settings.SetSetting(Settings.Favorite_Favorites,JsonConvert.SerializeObject(Favorites));
        }
    }
}
