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
using UniversalSend.Models.RestupModels;
using Windows.Storage;

namespace UniversalSend.Models
{
    [RestController(InstanceCreationType.PerCall)]
    public class V2RequestController
    {
        //[UriFormat("v2/hello")]
        //public GetResponse GetHello()
        //{
        //    Debug.WriteLine("GET /hello called"); // 调试输出
        //    return new GetResponse(
        //        GetResponse.ResponseStatus.OK,
        //        new DataReceived() { ID = 1, PropName = "Hello from UWP REST Server!" });
        //}

        //// 处理 POST /api/data  
        //[UriFormat("v2/data")]
        //public IPostResponse PostData([FromContent] dynamic data)
        //{
        //    string receivedData = data?.input; // 从请求体中获取数据  
        //    return new PostResponse(
        //        PostResponse.ResponseStatus.Created,
        //        receivedData); // 修复：直接传递字符串，而不是匿名类型  
        //}




        [UriFormat("v2/register")]
        public PostResponse PostRegister([FromContent] RegisterData data)
        {
            Debug.WriteLine("POST /register called"); // 调试输出
            if(data != null)
                Debug.WriteLine(JsonConvert.SerializeObject(data));
            return new PostResponse(
                PostResponse.ResponseStatus.Created,
                "",JsonConvert.SerializeObject(ProgramData.LocalDeviceRegisterData)); // 修复：直接传递字符串，而不是匿名类型  
        }

        //[UriFormat("v2/prepare-upload?fileId={fileId}&token={token}")]
        //public IPostResponse PostSendRequest(string fileId, string token, [FromContent] byte[] data)
        //{
        //    Debug.WriteLine($"POST send Called\nfileId = {fileId},token = {token},dataLength = {data.Length}B");
        //    SaveFileData(fileId, data);
        //    return new PostResponse(
        //        PostResponse.ResponseStatus.Created,
        //        ""
        //        );
        //}

        public async void SaveFileData(string fileId, byte[] data)
        {
            StorageFile storageFile = await StorageHelper.CreateFileInAppLocalFolderAsync(fileId);
            await StorageHelper.WriteBytesToFileAsync(storageFile, data);
        }
    }
}
