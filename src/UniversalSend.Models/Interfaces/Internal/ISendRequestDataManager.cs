using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces.Internal {
    internal interface ISendRequestDataManager {
        Task<ISendRequestData> CreateSendRequestDataAsync(List<IStorageFile> storageFiles);
    }
}