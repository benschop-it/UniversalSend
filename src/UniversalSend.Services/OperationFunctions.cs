using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restup.HttpMessage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace UniversalSend.Services
{
    public class OperationFunctions
    {
        // Handles incoming file from remote device
        public static async Task<object> SendRequestFuncAsync(MutableHttpServerRequest mutableHttpServerRequest)
        {
            Dictionary<string, string> queryParameters = StringHelper.GetURLQueryParameters(mutableHttpServerRequest.Uri.ToString());
            Debug.WriteLine($"Received file. Number of query parameters: {queryParameters.Count}");
            string fileId, token;
            if (!queryParameters.TryGetValue("fileId", out fileId) || !queryParameters.TryGetValue("token", out token))
            {
                ReceiveManager.SendDataReceivedEvent(null);
                return null;
            }

            ReceiveTask task = ReceiveTaskManager.WriteFileContentToReceivingTask(fileId, token, mutableHttpServerRequest.Content);

            if (task != null)
            {
                ReceiveManager.SendDataReceivedEvent(task);
                Debug.WriteLine("Writing data to file");
                var headerList = mutableHttpServerRequest.Headers.ToList();
                var item = headerList.Find(x => x.Name.Equals("host"));
                if (item == null)
                    return null;
                string host = item.Value;
                string ip = host.Substring(0, host.LastIndexOf(":"));
                await WriteFileAsync(task, ip);
            }
            else
            {
                ReceiveManager.SendDataReceivedEvent(null);
            }

            return null;
        }

        private static async Task WriteFileAsync(ReceiveTask task, string ip)
        {
            StorageFile file = await ReceiveTaskManager.WriteReceiveTaskToFileAsync(task);
            HistoryManager.AddHistoriesList(new History(task.file, StorageApplicationPermissions.FutureAccessList.Add(file), DeviceManager.CreateDeviceFromInfoData(task.sender)));
        }

        // Handles registration request from remote device
        public static object RegisterRequestFunc(MutableHttpServerRequest mutableHttpServerRequest)
        {
            var headerList = mutableHttpServerRequest.Headers.ToList();
            var item = headerList.Find(x => x.Name.Equals("host"));
            if (item == null)
                return null;

            string host = item.Value;
            string ip = host.Substring(0, host.LastIndexOf(":"));
            string portStr = host.Substring(host.LastIndexOf(":") + 1);
            int port = Convert.ToInt32(portStr);

            string jsonStr = StringHelper.ByteArrayToString(mutableHttpServerRequest.Content);

            RegisterRequestData registerRequestData = JsonConvert.DeserializeObject<RegisterRequestData>(jsonStr);
            if (registerRequestData == null)
                return null;

            Device device = new Device();
            device.Alias = registerRequestData.alias;
            device.HttpProtocol = "http";
            device.ProtocolVersion = "v1";
            device.DeviceModel = registerRequestData.deviceModel;
            device.DeviceType = registerRequestData.deviceType;
            device.Fingerprint = registerRequestData.fingerprint;
            device.IP = ip;
            device.Port = port;

            Register.NewDeviceRegisterV1Event(device);
            return null;
        }
    }
}
