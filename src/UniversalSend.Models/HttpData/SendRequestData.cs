using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using Windows.Storage;

namespace UniversalSend.Models.HttpData {

    public sealed class SendRequestData {

        #region Public Properties

        [JsonProperty("files")]
        public Dictionary<string, IFileRequestData> Files { get; set; }

        [JsonProperty("info")]
        public IInfoData Info { get; set; }

        #endregion Public Properties
    }

    internal class SendRequestDataManager {

        #region Public Methods

        public static async Task<SendRequestData> CreateSendRequestDataAsync(List<StorageFile> storageFiles) {
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