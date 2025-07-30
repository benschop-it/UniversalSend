using System.Collections.Generic;
using UniversalSend.Models.Interfaces;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace UniversalSend.Models.Helpers {

    internal class NetworkHelper : INetworkHelper {

        #region Public Methods

        public List<string> GetIPAddrList() {
            var hosts = NetworkInformation.GetHostNames();
            List<string> AddrList = new List<string>();
            foreach (var item in hosts) {
                if (IsHostIpaddr(item)) {
                    AddrList.Add(item.DisplayName);
                }
            }
            return AddrList;
        }

        public List<string> GetIPv4AddrList() {
            var hosts = NetworkInformation.GetHostNames();
            List<string> AddrList = new List<string>();
            foreach (var item in hosts) {
                if (IsHostIpaddr(item) && item.DisplayName.IndexOf(":") == -1) {
                    AddrList.Add(item.DisplayName);
                }
            }
            return AddrList;
        }

        #endregion Public Methods

        #region Private Methods

        private bool IsHostIpaddr(HostName hostName) {
            bool isIpaddr = (hostName.Type == HostNameType.Ipv4) || (hostName.Type == HostNameType.Ipv6);
            if (isIpaddr == false) {
                return false;
            }
            IPInformation ipinfo = hostName.IPInformation;
            if (ipinfo.NetworkAdapter.IanaInterfaceType == 71 || ipinfo.NetworkAdapter.IanaInterfaceType == 6) {
                return true;
            } else {
                return false;
            }
        }

        #endregion Private Methods
    }
}