using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace UniversalSend.Models
{
    public class NetworkHelper
    {
        public static List<string> GetIPAddrList()
        {
            var hosts = NetworkInformation.GetHostNames();
            List<string> AddrList = new List<string>();
            foreach(var item in hosts)
            {
                if(IsHostIpaddr(item))
                {
                    AddrList.Add(item.DisplayName);
                }
            }
            return AddrList;
        }

        public static List<string> GetIPv4AddrList()
        {
            var hosts = NetworkInformation.GetHostNames();
            List<string> AddrList = new List<string>();
            foreach (var item in hosts)
            {
                if (IsHostIpaddr(item) && item.DisplayName.IndexOf(":") == -1)
                {
                    AddrList.Add(item.DisplayName);
                }
            }
            return AddrList;
        }

        static bool IsHostIpaddr(HostName hostName)
        {
            bool isIpaddr = (hostName.Type == Windows.Networking.HostNameType.Ipv4) || (hostName.Type == Windows.Networking.HostNameType.Ipv6);
            if (isIpaddr == false)
            {
                return false;
            }
            IPInformation ipinfo = hostName.IPInformation;
            if (ipinfo.NetworkAdapter.IanaInterfaceType == 71 || ipinfo.NetworkAdapter.IanaInterfaceType == 6)
                return true;
            else
                return false;
        }
    }
}
