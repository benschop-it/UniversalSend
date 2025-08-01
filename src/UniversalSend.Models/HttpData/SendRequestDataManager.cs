using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.Managers;
using Windows.Storage;

namespace UniversalSend.Models.HttpData {
    public class SendRequestDataManager : ISendRequestDataManager {

        #region Public Methods

        public async Task<SendRequestData> CreateSendRequestDataAsync(List<StorageFile> storageFiles) {
            SendRequestData sendRequestData = new SendRequestData();
            sendRequestData.Info.Alias = ProgramData.LocalDevice.Alias;
            sendRequestData.Info.DeviceModel = ProgramData.LocalDevice.DeviceModel;
            sendRequestData.Info.DeviceType = ProgramData.LocalDevice.DeviceType;
            foreach (var file in storageFiles) {
                FileRequestData fileRequestData = await FileRequestDataManager.CreateFromStorageFileAsync(file);
                sendRequestData.Files.Add(fileRequestData.FileName, fileRequestData);
            }
            return sendRequestData;
        }

        #endregion Public Methods
    }
}