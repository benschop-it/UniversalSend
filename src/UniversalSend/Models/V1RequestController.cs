using Newtonsoft.Json;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.RestupModels;
using Windows.Storage;

namespace UniversalSend.Models
{
    [RestController(InstanceCreationType.PerCall)]
    public class V1RequestController
    {
        [UriFormat("v1/info?fingerprint={fingerprint}")]
        public GetResponse GetInfo(string fingerprint)
        {
            Debug.WriteLine($"GET v1/info Called\nfingerprint:{fingerprint}");
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                ProgramData.LocalDeviceInfoData
                ); // 返回本地设备信息
        }

        [UriFormat("v1/send-request")]
        public PostResponse PostSendRequest([FromContent]SendRequestData requestData)
        {
            Debug.WriteLine($"POST v1 send-request Called\nrequestdata:{JsonConvert.SerializeObject(requestData)}");
            FileResponseData responseData = new FileResponseData();
            if (requestData!=null && requestData.files!=null&&requestData.files.Count!=0)
            {
                foreach(var item in requestData.files)
                {
                    responseData.Add(item.Key, TokenFactory.CreateToken());
                }
            }
            //string responseDataString = JsonConvert.SerializeObject(responseData);
            //responseDataString = responseDataString.Replace("\\","");
            //Debug.WriteLine($"responseData:{responseDataString}");
            return new PostResponse(
                PostResponse.ResponseStatus.OK,
                "",
                responseData
                );
        }

        [UriFormat("v1/send?fileId={fileId}&token={token}")]
        public /*async Task<*/PostResponse/*>*/ PostSendRequest(string fileId, string token)
        {
            Debug.WriteLine($"POST send Called\nfileId = {fileId},token = {token},dataLength = B");/*{requestData.Length}*/
            //SaveFileData(fileId, data);
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
