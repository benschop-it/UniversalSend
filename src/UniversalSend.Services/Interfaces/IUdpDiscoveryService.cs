using System.Threading.Tasks;

namespace UniversalSend.Services.Interfaces {
    public interface IUdpDiscoveryService {
        Task SendAnnouncementAsyncV2();
    }
}