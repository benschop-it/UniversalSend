using Newtonsoft.Json;
using Restup.HttpMessage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace UniversalSend.Services {

    public class OperationFunctions {

        #region Public Methods

        // Handles registration request from remote device
        public static object RegisterRequestFunc(MutableHttpServerRequest mutableHttpServerRequest) {
            var headerList = mutableHttpServerRequest.Headers.ToList();
            var item = headerList.Find(x => x.Name.Equals("host"));
            if (item == null) {
                return null;
            }

            string host = item.Value;
            string ip = host.Substring(0, host.LastIndexOf(":"));
            string portStr = host.Substring(host.LastIndexOf(":") + 1);
            int port = Convert.ToInt32(portStr);

            string jsonStr = StringHelper.ByteArrayToString(mutableHttpServerRequest.Content);

            Debug.WriteLine($"RegisterRequestFunc: {host} {ip} {portStr} {jsonStr}");

            RegisterRequestData registerRequestData = JsonConvert.DeserializeObject<RegisterRequestData>(jsonStr);
            if (registerRequestData == null) {
                return null;
            }

            Device device = new Device {
                Alias = registerRequestData.Alias,
                HttpProtocol = "http",
                ProtocolVersion = "v1",
                DeviceModel = registerRequestData.DeviceModel,
                DeviceType = registerRequestData.DeviceType,
                Fingerprint = registerRequestData.Fingerprint,
                IP = ip,
                Port = port
            };

            Register.NewDeviceRegisterV1Event(device);
            return null;
        }

        // Handles incoming file from remote device
        public static async Task<object> SendRequestFuncAsync(MutableHttpServerRequest mutableHttpServerRequest) {
            Debug.WriteLine($"SendRequestFuncAsync {mutableHttpServerRequest.Uri.ToString()}");

            Dictionary<string, string> queryParameters = StringHelper.GetURLQueryParameters(mutableHttpServerRequest.Uri.ToString());
            Debug.WriteLine($"Received file. Number of query parameters: {queryParameters.Count}");

            string fileId, token;
            if (!queryParameters.TryGetValue("fileId", out fileId) || !queryParameters.TryGetValue("token", out token)) {
                ReceiveManager.SendDataReceivedEvent(null);
                return null;
            }

            ReceiveTask task = ReceiveTaskManager.WriteFileContentToReceivingTask(fileId, token, mutableHttpServerRequest.Content);

            if (task != null) {
                ReceiveManager.SendDataReceivedEvent(task);
                Debug.WriteLine("Writing data to file");
                var headerList = mutableHttpServerRequest.Headers.ToList();
                var item = headerList.Find(x => x.Name.Equals("host"));
                if (item == null) {
                    return null;
                }

                string host = item.Value;
                string ip = host.Substring(0, host.LastIndexOf(":"));
                await WriteFileAsync(task, ip);
            } else {
                ReceiveManager.SendDataReceivedEvent(null);
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private static async Task WriteFileAsync(ReceiveTask task, string ip) {
            StorageFile file = await ReceiveTaskManager.WriteReceiveTaskToFileAsync(task);
            var history = new History(
                task.File, StorageApplicationPermissions.FutureAccessList.Add(file),
                DeviceManager.CreateDeviceFromInfoData(task.Sender)
            );

            HistoryManager.AddHistoriesList(history);
        }

        #endregion Private Methods
    }
}