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
        //处理远程设备发送文件
        public static async Task<object> SendRequestFuncAsync(MutableHttpServerRequest mutableHttpServerRequest)
        {
            Dictionary<string,string> queryParameters = StringHelper.GetURLQueryParameters(mutableHttpServerRequest.Uri.ToString());
            Debug.WriteLine($"接收到文件 queryParameters个数：{queryParameters.Count}");
            string fileId,token;
            if (!queryParameters.TryGetValue("fileId", out fileId) || !queryParameters.TryGetValue("token", out token))
            {
                ReceiveManager.SendDataReceivedEvent(null);
                return null;
            }
            ReceiveTask task = ReceiveTaskManager.WriteFileContentToReceivingTask(fileId,token,mutableHttpServerRequest.Content);
            
            if(task != null)
            {
                ReceiveManager.SendDataReceivedEvent(task);
                Debug.WriteLine("正在写入数据至文件");
                var headerList = mutableHttpServerRequest.Headers.ToList();
                var item = headerList.Find(x => x.Name.Equals("host"));
                if (item == null)
                    return null;
                string host = item.Value;
                string ip = host.Substring(0, host.LastIndexOf(":"));
                await WriteFileAsync(task,ip);
            }
            else
            {
                ReceiveManager.SendDataReceivedEvent(null);
            }
            //    //byte[] fileContent = mutableHttpServerRequest.Content;

            //    //mutableHttpServerRequest
            //foreach (var item in mutableHttpServerRequest.Headers)
            //{
            //    Debug.WriteLine($"{item.Name}:{item.Value}");
            //}
            //mutableHttpServerRequest
            //ReceiveTaskManager.ReceivingTasks.Find(x=>x.file);
            
                return null;
        }

        private static async Task WriteFileAsync(ReceiveTask task,string ip)
        {
            StorageFile file = await ReceiveTaskManager.WriteReceiveTaskToFileAsync(task);
            HistoryManager.AddHistoriesList(new History(task.file, StorageApplicationPermissions.FutureAccessList.Add(file),DeviceManager.CreateDeviceFromInfoData(task.sender)));
        }

        //接收远程设备注册
        public static object RegisterRequestFunc(MutableHttpServerRequest mutableHttpServerRequest)
        {
            //Debug.WriteLine("------RegisterRequest------");
            //foreach(var item in mutableHttpServerRequest.Headers)
            //{
            //    Debug.WriteLine($"{item.Name}:{item.Value}");
            //}
            //Debug.WriteLine("------RegisterRequestEnd------");

            var headerList = mutableHttpServerRequest.Headers.ToList();
            var item = headerList.Find(x=>x.Name.Equals("host"));
            if (item == null)
                return null;
            string host = item.Value;
            string ip = host.Substring(0, host.LastIndexOf(":"));
            string portStr = host.Substring(host.LastIndexOf(":") +1);
            int port = Convert.ToInt32(portStr);
            

            string jsonStr = StringHelper.ByteArrayToString(mutableHttpServerRequest.Content);
            //JObject a = (JObject)JsonConvert.DeserializeObject(jsonStr);
            
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
