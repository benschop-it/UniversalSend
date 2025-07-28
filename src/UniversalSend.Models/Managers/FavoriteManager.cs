using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UniversalSend.Models.Managers {

    public class FavoriteManager {

        #region Public Properties

        public static List<Favorite> Favorites { get; set; } = new List<Favorite>();

        #endregion Public Properties

        #region Public Methods

        public static void InitFavoritesData() {
            string str = Settings.GetSettingContentAsString(Settings.Favorite_Favorites);
            if (String.IsNullOrEmpty(str))
                return;
            Favorites = JsonConvert.DeserializeObject<List<Favorite>>(str);
        }

        public static void SaveFavoritesData() {
            Settings.SetSetting(Settings.Favorite_Favorites, JsonConvert.SerializeObject(Favorites));
        }

        #endregion Public Methods
    }
}