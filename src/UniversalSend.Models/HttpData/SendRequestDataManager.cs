using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Managers;
using Windows.Storage;

namespace UniversalSend.Models.HttpData {
    internal class SendRequestDataManager : ISendRequestDataManager {

        private readonly IFileRequestDataManager _fileRequestDataManager;

        public SendRequestDataManager(IFileRequestDataManager fileRequestDataManager) {
            _fileRequestDataManager = fileRequestDataManager ?? throw new System.ArgumentNullException(nameof(fileRequestDataManager));
        }

        #region Public Methods

        public async Task<ISendRequestData> CreateSendRequestDataAsync(List<IStorageFile> storageFiles) {
            SendRequestData sendRequestData = new SendRequestData();
            sendRequestData.Info.Alias = ProgramData.LocalDevice.Alias;
            sendRequestData.Info.DeviceModel = ProgramData.LocalDevice.DeviceModel;
            sendRequestData.Info.DeviceType = ProgramData.LocalDevice.DeviceType;
            foreach (var file in storageFiles) {
                IFileRequestData fileRequestData = await _fileRequestDataManager.CreateFromStorageFileAsync(file);
                sendRequestData.Files.Add(fileRequestData.FileName, (FileRequestData)fileRequestData);
            }
            return sendRequestData;
        }

        #endregion Public Methods
    }
}