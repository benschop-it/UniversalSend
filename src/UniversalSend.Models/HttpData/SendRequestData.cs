using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.HttpData
{
    public sealed class SendRequestData
    {
        public InfoData info { get; set; }
        public Dictionary<string, FileRequestData>files { get;set; }
    }

    public class SendRequestDataManager
    {
        public static async Task<SendRequestData> CreateSendRequestDataAsync(List<StorageFile>storageFiles)
        {
            SendRequestData sendRequestData = new SendRequestData();
            sendRequestData.info.alias = ProgramData.LocalDevice.Alias;
            sendRequestData.info.deviceModel = ProgramData.LocalDevice.DeviceModel;
            sendRequestData.info.deviceType = ProgramData.LocalDevice.DeviceType;
            foreach(var file in storageFiles)
            {
                FileRequestData fileRequestData = await FileRequestDataManager.CreateFromStorageFileAsync(file);

                sendRequestData.files.Add(fileRequestData.fileName, fileRequestData);
            }
            return sendRequestData;
        }
    }
}
