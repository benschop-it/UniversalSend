using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models {

    public class Favorite : IFavorite {

        #region Public Constructors

        public Favorite(string deviceName, string ipAddr, long port) {
            DeviceName = deviceName;
            IPAddr = ipAddr;
            Port = port;
        }

        #endregion Public Constructors

        #region Public Properties

        public string DeviceName { get; set; }

        public string IPAddr { get; set; }

        public long Port { get; set; }

        #endregion Public Properties
    }
}