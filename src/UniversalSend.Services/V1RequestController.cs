using Newtonsoft.Json;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.Helpers;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Managers;
using UniversalSend.Models.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace UniversalSend.Services {

    [RestController(InstanceCreationType.PerCall)]
    public class V1RequestController {

        #region Public Methods

        [UriFormat("v1/info?fingerprint={fingerprint}")]
        public GetResponse GetInfo(string fingerprint) {
            Debug.WriteLine($"GET v1/info Called\nfingerprint:{fingerprint}");
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                InfoDataManager.GetInfoDataFromDevice(ProgramData.LocalDevice)
            );
        }

        [UriFormat("v1/cancel")]
        public PostResponse PostCancel() {
            Debug.WriteLine($"GET v1/Cancel Called");
            ReceiveManager.CancelReceivedEvent();
            return new PostResponse(
                PostResponse.ResponseStatus.OK,
                ""
            );
        }

        [UriFormat("v1/register")]
        public PostResponse PostRegister([FromContent]RegisterRequestData registerRequestData) {
            Debug.WriteLine($"POST v1 register request Called: {registerRequestData.Alias} {registerRequestData.DeviceModel} {registerRequestData.DeviceType} {registerRequestData.Fingerprint}");

            return new PostResponse(
                PostResponse.ResponseStatus.OK,
                "",
                new RegisterResponseData {
                    Alias = ProgramData.LocalDevice.Alias,
                    DeviceModel = ProgramData.LocalDevice.DeviceModel,
                    DeviceType = ProgramData.LocalDevice.DeviceType,
                    Fingerprint = ProgramData.LocalDevice.Fingerprint,
                    Announcement = false
                }
            );
        }

        [UriFormat("v1/send-request")]
        public IAsyncOperation<IPostResponse> PostSendRequest([FromContent]SendRequestData requestData) {
            Debug.WriteLine($"POST v1 send-request Called\nrequestdata:{JsonConvert.SerializeObject(requestData)}");

            ReceiveManager.SendRequestEvent(requestData);

            FileResponseData responseData = new FileResponseData();
            if (requestData != null && requestData.Files != null && requestData.Files.Count != 0) {
                foreach (var item in requestData.Files) {
                    string token = TokenFactory.CreateToken();
                    responseData.Add(item.Key, token);
                    ReceiveTaskManager.CreateReceivingTaskFromUniversalSendFile(
                        UniversalSendFileManager.GetUniversalSendFileFromFileRequestDataAndToken(item.Value, token),
                        requestData.Info
                    );
                }
            }

            return Task.FromResult<IPostResponse>(new PostResponse(
                PostResponse.ResponseStatus.OK,
                "",
                (object)responseData // Cast to object otherwise the wrong method will be called!
            )).AsAsyncOperation();
        }

        [UriFormat("v1/send?fileId={fileId}&token={token}")]
        public IAsyncOperation<IPostResponse> PostSendRequest(string fileId, string token) {
            Debug.WriteLine($"POST send Called\nfileId = {fileId},token = {token},dataLength = B");
            return Task.FromResult<IPostResponse>(new PostResponse(
                PostResponse.ResponseStatus.OK,
                ""
            )).AsAsyncOperation();
        }

        #endregion Public Methods
    }
}