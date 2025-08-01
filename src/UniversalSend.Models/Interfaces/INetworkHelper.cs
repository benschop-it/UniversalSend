using System.Collections.Generic;

namespace UniversalSend.Models.Interfaces {
    public interface INetworkHelper {

        #region Public Methods

        List<string> GetIPAddrList();
        List<string> GetIPv4AddrList();

        #endregion Public Methods
    }
}