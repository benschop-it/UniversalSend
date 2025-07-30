using System.Collections.Generic;

namespace UniversalSend.Models.Interfaces {
    public interface IFavoriteManager {
        List<IFavorite> Favorites { get; set; }

        void InitFavoritesData();
        void SaveFavoritesData();

        IFavorite CreateFavorite(string deviceName, string ipAddr, long port);
    }
}