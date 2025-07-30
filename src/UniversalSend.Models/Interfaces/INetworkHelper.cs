using System.Collections.Generic;

namespace UniversalSend.Models.Interfaces {
    public interface INetworkHelper {
        List<string> GetIPAddrList();
        List<string> GetIPv4AddrList();
    }
}