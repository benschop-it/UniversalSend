using Restup.HttpMessage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Tasks;

namespace UniversalSend.Services
{
    public class OperationFunctions
    {
        public static object SendRequestFunc(MutableHttpServerRequest mutableHttpServerRequest)
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
                Debug.WriteLine($"接收到文件，写入byte[]数据成功");
                ReceiveManager.SendDataReceivedEvent(task);
                Debug.WriteLine("正在写入数据至文件");
                ReceiveTaskManager.WriteReceiveTaskToFileAsync(task);
            }else
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
    }
}
