using System.Collections.Generic;

namespace UniversalSend.Models.Interfaces {
    public interface IFavoriteManager {

        #region Public Properties

        List<IFavorite> Favorites { get; set; }

        #endregion Public Properties

        #region Public Methods

        IFavorite CreateFavorite(string deviceName, string ipAddr, long port);

        void InitFavoritesData();
        void SaveFavoritesData();

        #endregion Public Methods
    }
}