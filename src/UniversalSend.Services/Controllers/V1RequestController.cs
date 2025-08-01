using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Attributes;
using UniversalSend.Services.Models.Schemas;
using UniversalSend.Services.Rest.Models.Contracts;
using Windows.Foundation;

namespace UniversalSend.Services.Controllers {

    [RestController(InstanceCreationType.PerCall)]
    internal class V1RequestController {

        #region Private Fields

        private IInfoDataManager _infoDataManager;
        private IReceiveManager _receiveManager;
        private IReceiveTaskManager _receiveTaskManager;
        private IRegisterResponseDataManager _registerResponseDataManager;
        private ITokenFactory _tokenFactory;
        private IUniversalSendFileManager _universalSendFileManager;

        #endregion Private Fields

        #region Public Constructors

        public V1RequestController(
            IInfoDataManager infoDataManager,
            IReceiveManager receiveManager,
            ITokenFactory tokenFactory,
            IUniversalSendFileManager universalSendFileManager,
            IReceiveTaskManager receiveTaskManager,
            IRegisterResponseDataManager registerResponseDataManager
        ) {
            _infoDataManager = infoDataManager ?? throw new ArgumentNullException(nameof(infoDataManager));
            _receiveManager = receiveManager ?? throw new ArgumentNullException(nameof(receiveManager));
            _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
            _universalSendFileManager = universalSendFileManager ?? throw new ArgumentNullException(nameof(universalSendFileManager));
            _receiveTaskManager = receiveTaskManager ?? throw new ArgumentNullException(nameof(receiveTaskManager));
            _registerResponseDataManager = registerResponseDataManager ?? throw new ArgumentNullException(nameof(registerResponseDataManager));
        }

        #endregion Public Constructors

        #region Public Methods

        [UriFormat("v1/info?fingerprint={fingerprint}")]
        public GetResponse GetInfo(string fingerprint) {
            Debug.WriteLine($"GET v1/info Called\nfingerprint:{fingerprint}");
            return new GetResponse(
                GetResponse.ResponseStatus.OK,
                _infoDataManager.GetInfoDataFromDevice()
            );
        }

        [UriFormat("v1/cancel")]
        public PostResponse PostCancel() {
            Debug.WriteLine($"GET v1/Cancel Called");
            _receiveManager.CancelReceivedEvent();
            return new PostResponse(
                PostResponse.ResponseStatus.OK,
                ""
            );
        }

        [UriFormat("v1/register")]
        public PostResponse PostRegister([FromContent] RegisterRequestData registerRequestData) {
            Debug.WriteLine($"POST v1 register request Called: {registerRequestData.Alias} {registerRequestData.DeviceModel} {registerRequestData.DeviceType} {registerRequestData.Fingerprint}");

            IRegisterResponseData registerResponseData = _registerResponseDataManager.GetRegisterReponseData(false);
            return new PostResponse(PostResponse.ResponseStatus.OK, "", registerResponseData);
        }

        [UriFormat("v1/send-request")]
        public IAsyncOperation<IPostResponse> PostSendRequest([FromContent] SendRequestData requestData) {
            Debug.WriteLine($"POST v1 send-request Called\nrequestdata:{JsonConvert.SerializeObject(requestData)}");

            _receiveManager.SendRequestEvent(requestData);

            FileResponseData responseData = new FileResponseData();
            if (requestData != null && requestData.Files != null && requestData.Files.Count != 0) {
                foreach (var item in requestData.Files) {
                    string token = _tokenFactory.CreateToken();
                    responseData.Add(item.Key, token);
                    _receiveTaskManager.CreateReceivingTaskFromUniversalSendFile(
                        _universalSendFileManager.GetUniversalSendFileFromFileRequestDataAndToken(item.Value, token),
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