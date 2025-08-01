using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {
    public interface ISendRequestDataManager {
        Task<ISendRequestData> CreateSendRequestDataAsync(List<IStorageFile> storageFiles);
    }
}