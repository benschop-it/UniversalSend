using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Interfaces.Internal;
using UniversalSend.Models.Managers;
using Windows.Storage;

namespace UniversalSend.Models.HttpData {
    internal class SendRequestDataManager : ISendRequestDataManager {

        #region Private Fields

        private readonly IFileRequestDataManager _fileRequestDataManager;

        #endregion Private Fields

        #region Public Constructors

        public SendRequestDataManager(IFileRequestDataManager fileRequestDataManager) {
            _fileRequestDataManager = fileRequestDataManager ?? throw new System.ArgumentNullException(nameof(fileRequestDataManager));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<ISendRequestDataV1> CreateSendRequestDataV1Async(List<IStorageFile> storageFiles) {
            SendRequestDataV1 sendRequestData = new SendRequestDataV1();
            sendRequestData.Info.Alias = ProgramData.LocalDevice.Alias;
            sendRequestData.Info.DeviceModel = ProgramData.LocalDevice.DeviceModel;
            sendRequestData.Info.DeviceType = ProgramData.LocalDevice.DeviceType;
            foreach (var file in storageFiles) {
                IFileRequestDataV1 fileRequestData = await _fileRequestDataManager.CreateFromStorageFileV1Async(file);
                sendRequestData.Files.Add(fileRequestData.FileName, (FileRequestDataV1)fileRequestData);
            }
            return sendRequestData;
        }

        public async Task<ISendRequestDataV2> CreateSendRequestDataV2Async(List<IStorageFile> storageFiles) {
            SendRequestDataV2 sendRequestData = new SendRequestDataV2();
            sendRequestData.Info.Alias = ProgramData.LocalDevice.Alias;
            sendRequestData.Info.DeviceModel = ProgramData.LocalDevice.DeviceModel;
            sendRequestData.Info.DeviceType = ProgramData.LocalDevice.DeviceType;
            foreach (var file in storageFiles) {
                IFileRequestDataV2 fileRequestData = await _fileRequestDataManager.CreateFromStorageFileV2Async(file);
                sendRequestData.Files.Add(fileRequestData.FileName, (FileRequestDataV2)fileRequestData);
            }
            return sendRequestData;
        }


        #endregion Public Methods

    }
}