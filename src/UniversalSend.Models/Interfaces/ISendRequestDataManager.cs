using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.HttpData {
    public interface ISendRequestDataManager {
        Task<SendRequestData> CreateSendRequestDataAsync(List<StorageFile> storageFiles);
    }
}