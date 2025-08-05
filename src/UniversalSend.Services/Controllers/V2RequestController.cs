using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Models.Common;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Attributes;
using UniversalSend.Services.Models.Schemas;
using UniversalSend.Services.Rest.Models.Contracts;
using Windows.Foundation;

namespace UniversalSend.Services.Controllers {

    [RestController(InstanceCreationType.PerCall)]
    internal class V2RequestController {

        #region Private Fields

        private readonly ILogger _logger;
        private readonly IInfoDataManager _infoDataManager;
        private readonly IReceiveManager _receiveManager;
        private readonly IReceiveTaskManager _receiveTaskManager;
        private readonly IRegisterResponseDataManager _registerResponseDataManager;
        private readonly ITokenFactory _tokenFactory;
        private readonly IUniversalSendFileManager _universalSendFileManager;

        #endregion Private Fields

        #region Public Constructors

        public V2RequestController(
            IInfoDataManager infoDataManager,
            IReceiveManager receiveManager,
            ITokenFactory tokenFactory,
            IUniversalSendFileManager universalSendFileManager,
            IReceiveTaskManager receiveTaskManager,
            IRegisterResponseDataManager registerResponseDataManager
        ) {
            _logger = LogManager.GetLogger<V2RequestController>();
            _infoDataManager = infoDataManager ?? throw new ArgumentNullException(nameof(infoDataManager));
            _receiveManager = receiveManager ?? throw new ArgumentNullException(nameof(receiveManager));
            _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
            _universalSendFileManager = universalSendFileManager ?? throw new ArgumentNullException(nameof(universalSendFileManager));
            _receiveTaskManager = receiveTaskManager ?? throw new ArgumentNullException(nameof(receiveTaskManager));
            _registerResponseDataManager = registerResponseDataManager ?? throw new ArgumentNullException(nameof(registerResponseDataManager));
        }

        #endregion Public Constructors

        #region Public Methods

        [UriFormat("v2/info?fingerprint={fingerprint}")]
        public GetResponse GetInfo(string fingerprint) {
            _logger.Debug($"GET v2/info called with fingerprint: {fingerprint}.");

            return new GetResponse(GetResponse.ResponseStatus.OK, _infoDataManager.GetInfoDataV2FromDevice());
        }

        [UriFormat("v2/cancel?sessionId={sessionId}")]
        public PostResponse PostCancel(string sessionId) {
            _logger.Debug($"GET v2/Cancel called for sessionId {sessionId}.");

            _receiveManager.CancelReceivedEvent();
            return new PostResponse(PostResponse.ResponseStatus.OK, "");
        }

        [UriFormat("v2/register")]
        public PostResponse PostRegister([FromContent] RegisterRequestDataV2 registerRequestData) {
            _logger.Debug($"POST v2 register request called with RegisterRequestData:\n{JsonConvert.SerializeObject(registerRequestData)}.");

            IRegisterResponseDataV2 registerResponseData = _registerResponseDataManager.GetRegisterResponseDataV2();
            return new PostResponse(PostResponse.ResponseStatus.OK, "", registerResponseData);
        }

        [UriFormat("v2/prepare-upload")]
        public IAsyncOperation<IPostResponse> PostSendRequestAsync([FromContent] SendRequestDataV2 requestData) {
            return AsyncInfo.Run(async ct => {
                _logger.Debug($"POST v2 send-request called with SendRequestData:\n{JsonConvert.SerializeObject(requestData)}.");

                var sessionId = _tokenFactory.CreateToken();
                _receiveManager.SendRequestV2Event(requestData);

                FileResponseDataV2 responseData = new FileResponseDataV2();
                responseData.SessionId = sessionId;

                if (requestData?.Files != null && requestData.Files.Count != 0) {
                    foreach (var item in requestData.Files) {
                        var token = _tokenFactory.CreateToken();
                        responseData.Files.Add(item.Key, token);
                        var file = _universalSendFileManager.GetUniversalSendFileFromFileRequestDataV2AndToken(item.Value, token);
                        await _receiveTaskManager.CreateReceivingTaskFromUniversalSendFileV2Async(file, requestData.Info);
                    }
                }

                return (IPostResponse)new PostResponse(PostResponse.ResponseStatus.OK, "", (object)responseData);
            });
        }

        [UriFormat("v2/upload?sessionId={sessionId}&fileId={fileId}&token={token}")]
        public IAsyncOperation<IPostResponse> PostSendRequest(string sessionId, string fileId, string token) {
            _logger.Debug($"POST send called with sessionId = {sessionId} fileId = {fileId} and token = {token}.");

            return Task.FromResult<IPostResponse>(
                new PostResponse(
                    PostResponse.ResponseStatus.OK,
                    ""
                )
            ).AsAsyncOperation();
        }

        #endregion Public Methods

    }
}