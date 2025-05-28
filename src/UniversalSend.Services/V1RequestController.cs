using Newtonsoft.Json;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace UniversalSend.Services
{
    [RestController(InstanceCreationType.PerCall)]
    public class V1RequestController
    {
        [UriFormat("v1/register")]
        public PostResponse PostRegister([FromContent]RegisterRequestData registerRequestData)
        {

            return new PostResponse(
                PostResponse.ResponseStatus.OK,
                "",
                new RegisterResponseData
                {
                    alias = ProgramData.LocalDevice.Alias,
                    deviceModel = ProgramData.LocalDevice.DeviceModel,
                    deviceType = ProgramData.LocalDevice.DeviceType,
                    fingerprint = ProgramData.LocalDevice.Fingerprint,
                    announcement = false
                }
                );
        }

        [UriFormat("v1/info?fingerprint={fingerprint}")]
        public GetResponse GetInfo(string fingerprint)
        {
            Debug.WriteLine($"GET v1/info Called\nfingerprint:{fingerprint}");
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                InfoDataManager.GetInfoDataFromDevice(ProgramData.LocalDevice)//ProgramData.LocalDeviceInfoData
                ); // 返回本地设备信息
        }

        [UriFormat("v1/send-request")]
        public IAsyncOperation<IPostResponse> PostSendRequest([FromContent]SendRequestData requestData)
        {
            Debug.WriteLine($"POST v1 send-request Called\nrequestdata:{JsonConvert.SerializeObject(requestData)}");
            ReceiveManager.SendRequestEvent(requestData);
            
            FileResponseData responseData = new FileResponseData();
            if (requestData!=null && requestData.files!=null&&requestData.files.Count!=0)
            {
                foreach(var item in requestData.files)
                {
                    string token = TokenFactory.CreateToken();
                    responseData.Add(item.Key, token);
                    ReceiveTaskManager.CreateReceivingTaskFromUniversalSendFile(UniversalSendFileManager.GetUniversalSendFileFromFileRequestDataAndToken(item.Value, token),requestData.info);
                }
            }
            //if (ReceiveManager.QuickSave == ReceiveManager.QuickSaveMode.Off)
            //{
            //    if (await ReceiveManager.GetChosenOption())
            //    {
            //        return Task.FromResult<IPostResponse>(new PostResponse(
            //        PostResponse.ResponseStatus.OK,
            //        "",
            //        responseData
            //        )).AsAsyncOperation();
            //    }
            //    else
            //    {
            //        return Task.FromResult<IPostResponse>(new PostResponse(
            //        PostResponse.ResponseStatus.OK,
            //        "",
            //        new FileResponseData()
            //        )).AsAsyncOperation();
            //    }
            //}
            //else
            //{
            return Task.FromResult<IPostResponse>(new PostResponse(
                PostResponse.ResponseStatus.OK,
                "",
                responseData
                )).AsAsyncOperation();
            //}
            //string responseDataString = JsonConvert.SerializeObject(responseData);
            //responseDataString = responseDataString.Replace("\\","");
            //Debug.WriteLine($"responseData:{responseDataString}");

        }

        [UriFormat("v1/send?fileId={fileId}&token={token}")]
        public IAsyncOperation<IPostResponse> PostSendRequest(string fileId, string token)
        {
            Debug.WriteLine($"POST send Called\nfileId = {fileId},token = {token},dataLength = B");/*{requestData.Length}*/
            //SaveFileData(fileId, data);
            //ReceiveTask task = ReceiveTaskManager.ReceivingTasks.Find(x=>x.file.Id.Equals(fileId));
            //if(task != null)
            //    task.TaskState = ReceiveTask.ReceiveTaskStates.Receiving;
            return Task.FromResult<IPostResponse>(new PostResponse(
                PostResponse.ResponseStatus.OK,
                ""
                )).AsAsyncOperation();
        }

        
        [UriFormat("v1/cancel")]
        public PostResponse PostCancel()
        {
            Debug.WriteLine($"GET v1/Cancel Called");
            ReceiveManager.CancelReceivedEvent();
            return new PostResponse(
                PostResponse.ResponseStatus.OK,
                ""
                );
        }

        public async void SaveFileData(string fileId,byte[]data)
        {
            StorageFile storageFile = await StorageHelper.CreateFileInAppLocalFolderAsync(fileId);
            await StorageHelper.WriteBytesToFileAsync(storageFile, data);
        }

        
    }
}
