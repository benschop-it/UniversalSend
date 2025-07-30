using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UniversalSend.Models.Interfaces;
using UniversalSend.Strings;

namespace UniversalSend.Models.Managers {

    public class FavoriteManager : IFavoriteManager {

        private ISettings _settings;

        #region Public Properties

        public List<IFavorite> Favorites { get; set; } = new List<IFavorite>();

        #endregion Public Properties

        public FavoriteManager(ISettings settings) {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #region Public Methods

        public void InitFavoritesData() {
            string str = _settings.GetSettingContentAsString(Constants.Favorite_Favorites);
            if (String.IsNullOrEmpty(str)) {
                return;
            }

            Favorites = JsonConvert.DeserializeObject<List<IFavorite>>(str);
        }

        public void SaveFavoritesData() {
            _settings.SetSetting(Constants.Favorite_Favorites, JsonConvert.SerializeObject(Favorites));
        }

        public IFavorite CreateFavorite(string deviceName, string ipAddr, long port) {
            return new Favorite(deviceName, ipAddr, port);
        }

        #endregion Public Methods
    }
}