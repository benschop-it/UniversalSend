using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {
    public interface ISendTaskManager {

        #region Public Properties

        List<ISendTaskV1> SendTasksV1 { get; }
        List<ISendTaskV2> SendTasksV2 { get; }

        #endregion Public Properties

        #region Public Methods

        Task<ISendTaskV1> CreateSendTaskV1(IStorageFile file);
        Task<ISendTaskV2> CreateSendTaskV2(IStorageFile file);
        ISendTaskV1 CreateSendTaskV1(string text);
        ISendTaskV2 CreateSendTaskV2(string text);
        ISendTaskV1 CreateSendTaskFromFileRequestDataAndStorageFileV1(IFileRequestDataV1 fileRequestData, IStorageFile storageFile);
        ISendTaskV2 CreateSendTaskFromFileRequestDataAndStorageFileV2(IFileRequestDataV2 fileRequestData, IStorageFile storageFile);
        Task CreateSendTasksV1(List<IStorageFile> files);
        Task CreateSendTasksV2(List<IStorageFile> files);
        Task<bool> SendSendRequestV1Async(IDevice destinationDevice);
        Task<bool> SendSendRequestV2Async(IDevice destinationDevice);
        Task SendSendTasksV1Async(IDevice destinationDevice);
        Task SendSendTasksV2Async(IDevice destinationDevice);

        #endregion Public Methods
    }
}