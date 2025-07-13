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
using UniversalSend.Models.HttpData;
using Windows.Storage;

namespace UniversalSend.Services
{
    [RestController(InstanceCreationType.PerCall)]
    public class V2RequestController
    {
        //[UriFormat("v2/hello")]
        //public GetResponse GetHello()
        //{
        //    Debug.WriteLine("GET /hello called"); // Debug output
        //    return new GetResponse(
        //        GetResponse.ResponseStatus.OK,
        //        new DataReceived() { ID = 1, PropName = "Hello from UWP REST Server!" });
        //}

        //// Handle POST /api/data  
        //[UriFormat("v2/data")]
        //public IPostResponse PostData([FromContent] dynamic data)
        //{
        //    string receivedData = data?.input; // Get data from request body  
        //    return new PostResponse(
        //        PostResponse.ResponseStatus.Created,
        //        receivedData); // Fix: pass string directly instead of anonymous type  
        //}




        [UriFormat("v2/register")]
        public PostResponse PostRegister([FromContent] RegisterData data)
        {
            Debug.WriteLine("POST /register called"); // Debug output
            if (data != null)
                Debug.WriteLine(JsonConvert.SerializeObject(data));
            return new PostResponse(
                PostResponse.ResponseStatus.Created,
                "", JsonConvert.SerializeObject(RegisterDataManager.GetRegisterDataFromDevice(ProgramData.LocalDevice)/*ProgramData.LocalDeviceRegisterData*/)); // Fix: pass string directly instead of anonymous type  
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
