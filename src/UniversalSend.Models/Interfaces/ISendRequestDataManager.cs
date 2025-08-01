using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using Windows.Storage;

namespace UniversalSend.Models.HttpData {
    public interface ISendRequestDataManager {
        Task<ISendRequestData> CreateSendRequestDataAsync(List<IStorageFile> storageFiles);
    }
}