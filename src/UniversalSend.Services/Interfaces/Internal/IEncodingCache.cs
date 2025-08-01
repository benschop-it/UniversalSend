using System.Text;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IEncodingCache {

        #region Public Methods

        Encoding GetEncoding(string charset);

        #endregion Public Methods
    }
}