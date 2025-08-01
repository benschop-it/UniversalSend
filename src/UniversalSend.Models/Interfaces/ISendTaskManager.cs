using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {
    public interface ISendTaskManager {

        #region Public Properties

        List<ISendTask> SendTasks { get; }

        #endregion Public Properties

        #region Public Methods

        Task<ISendTask> CreateSendTask(IStorageFile file);
        ISendTask CreateSendTask(string text);
        ISendTask CreateSendTaskFromFileRequestDataAndStorageFile(IFileRequestData fileRequestData, IStorageFile storageFile);
        Task CreateSendTasks(List<IStorageFile> files);
        Task<bool> SendSendRequestAsync(IDevice destinationDevice);
        Task SendSendTasksAsync(IDevice destinationDevice);

        #endregion Public Methods
    }
}