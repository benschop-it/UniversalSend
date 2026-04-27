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
        private readonly IConfirmReceiptHandler _confirmReceiptHandler;
        private readonly IWebSendManager _webSendManager;
        private readonly IStorageHelper _storageHelper;
        private readonly IFileRequestDataManager _fileRequestDataManager;

        #endregion Private Fields

        #region Public Constructors

        public V2RequestController(
            IInfoDataManager infoDataManager,
            IReceiveManager receiveManager,
            ITokenFactory tokenFactory,
            IUniversalSendFileManager universalSendFileManager,
            IReceiveTaskManager receiveTaskManager,
            IRegisterResponseDataManager registerResponseDataManager,
            IConfirmReceiptHandler confirmReceiptHandler,
            IWebSendManager webSendManager,
            IStorageHelper storageHelper,
            IFileRequestDataManager fileRequestDataManager
        ) {
            _logger = LogManager.GetLogger<V2RequestController>();
            _infoDataManager = infoDataManager ?? throw new ArgumentNullException(nameof(infoDataManager));
            _receiveManager = receiveManager ?? throw new ArgumentNullException(nameof(receiveManager));
            _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
            _universalSendFileManager = universalSendFileManager ?? throw new ArgumentNullException(nameof(universalSendFileManager));
            _receiveTaskManager = receiveTaskManager ?? throw new ArgumentNullException(nameof(receiveTaskManager));
            _registerResponseDataManager = registerResponseDataManager ?? throw new ArgumentNullException(nameof(registerResponseDataManager));
            _confirmReceiptHandler = confirmReceiptHandler ?? throw new ArgumentNullException(nameof(confirmReceiptHandler));
            _webSendManager = webSendManager ?? throw new ArgumentNullException(nameof(webSendManager));
            _storageHelper = storageHelper ?? throw new ArgumentNullException(nameof(storageHelper));
            _fileRequestDataManager = fileRequestDataManager ?? throw new ArgumentNullException(nameof(fileRequestDataManager));
        }

        #endregion Public Constructors

        #region Public Methods

        [UriFormat("v2/info")]
        public GetResponse GetInfo() {
            _logger.Debug("GET v2/info called.");

            return new GetResponse(GetResponse.ResponseStatus.OK, _infoDataManager.GetInfoDataV2FromDevice());
        }

        [UriFormat("v2/info?fingerprint={fingerprint}")]
        public GetResponse GetInfoWithFingerprint(string fingerprint) {
            _logger.Debug($"GET v2/info called with fingerprint: {fingerprint}.");

            return new GetResponse(GetResponse.ResponseStatus.OK, _infoDataManager.GetInfoDataV2FromDevice());
        }

        [UriFormat("v1/info?fingerprint={fingerprint}")]
        public GetResponse GetInfoV1(string fingerprint) {
            _logger.Debug($"GET v1/info called with fingerprint: {fingerprint}.");

            return new GetResponse(GetResponse.ResponseStatus.OK, _infoDataManager.GetInfoDataV2FromDevice());
        }

        [UriFormat("v2/cancel")]
        public IAsyncOperation<IPostResponse> PostCancel() {
            return AsyncInfo.Run(async ct => {
                _logger.Debug($"GET v2/Cancel called.");

                var isCanceled = await _confirmReceiptHandler.CancelAsync();
                if (!isCanceled) {
                    return new PostResponse(PostResponse.ResponseStatus.Rejected);
                }
                _receiveManager.CancelReceivedEvent();

                return (IPostResponse)new PostResponse(PostResponse.ResponseStatus.OK, "");
            });
        }
   
        [UriFormat("v2/cancel?sessionId={sessionId}")]
        public PostResponse PostCancel(string sessionId) {
            _logger.Debug($"GET v2/Cancel called for sessionId {sessionId}.");

            if (!_receiveTaskManager.CancelReceivingSession(sessionId)) {
                return new PostResponse(PostResponse.ResponseStatus.InvalidTokenOrIp);
            }

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

                if (!_receiveTaskManager.TryStartReceivingSession(sessionId)) {
                    return new PostResponse(PostResponse.ResponseStatus.BlockedByOtherSession);
                }

                var isAccepted = await _confirmReceiptHandler.ConfirmAsync(requestData);
                if (!isAccepted) {
                    _receiveTaskManager.CancelReceivingSession(sessionId);
                    return new PostResponse(PostResponse.ResponseStatus.Rejected);
                }

                if (requestData?.Files == null || requestData.Files.Count == 0) {
                    _receiveTaskManager.CancelReceivingSession(sessionId);
                    return new PostResponse(PostResponse.ResponseStatus.Finished);
                }

                _receiveManager.SendRequestV2Event(requestData);

                FileResponseDataV2 responseData = new FileResponseDataV2();
                responseData.SessionId = sessionId;

                if (requestData?.Files != null && requestData.Files.Count != 0) {
                    foreach (var item in requestData.Files) {
                        var token = _tokenFactory.CreateToken();
                        responseData.Files.Add(item.Key, token);
                        var file = _universalSendFileManager.GetUniversalSendFileFromFileRequestDataV2AndToken(item.Value, token);
                        await _receiveTaskManager.CreateReceivingTaskFromUniversalSendFileV2Async(file, requestData.Info, sessionId);
                    }
                }

                return (IPostResponse)new PostResponse(PostResponse.ResponseStatus.OK, "", (object)responseData);
            });
        }

        [UriFormat("v2/upload?sessionId={sessionId}&fileId={fileId}&token={token}")]
        public IAsyncOperation<IPostResponse> PostSendRequest(string sessionId, string fileId, string token) {
            _logger.Debug($"POST send called with sessionId = {sessionId} fileId = {fileId} and token = {token}.");

            PostResponse.ResponseStatus status = PostResponse.ResponseStatus.OK;
            switch (_receiveTaskManager.ValidateUploadRequest(sessionId, fileId, token)) {
                case UploadRequestValidationResult.MissingParameters:
                    status = PostResponse.ResponseStatus.MissingParameters;
                    break;
                case UploadRequestValidationResult.InvalidTokenOrSession:
                    status = PostResponse.ResponseStatus.InvalidTokenOrIp;
                    break;
                case UploadRequestValidationResult.BlockedByOtherSession:
                    status = PostResponse.ResponseStatus.BlockedByOtherSession;
                    break;
            }

            return Task.FromResult<IPostResponse>(
                new PostResponse(
                    status,
                    ""
                )
            ).AsAsyncOperation();
        }

        [UriFormat("v2/prepare-download")]
        public PostResponse PostPrepareDownload() {
            var share = _webSendManager.GetActiveShare();
            if (share == null) {
                return new PostResponse(PostResponse.ResponseStatus.Rejected);
            }

            var responseData = new DownloadPrepareResponseData {
                Info = _infoDataManager.GetInfoDataV2FromDevice(),
                SessionId = share.SessionId,
            };

            foreach (var item in share.Files) {
                responseData.Files[item.Key] = _fileRequestDataManager.CreateFromUniversalSendFileV2(item.Value.File);
            }

            return new PostResponse(PostResponse.ResponseStatus.OK, string.Empty, responseData);
        }

        [UriFormat("v2/download?sessionId={sessionId}&fileId={fileId}")]
        public IGetResponse GetDownload(string sessionId, string fileId) {
            var share = _webSendManager.GetActiveShare();
            if (share == null || !string.Equals(share.SessionId, sessionId, StringComparison.Ordinal) || !share.Files.TryGetValue(fileId, out ISendTaskV2 sendTask)) {
                return new GetResponse(GetResponse.ResponseStatus.NotFound);
            }

            return new BinaryGetResponse(GetResponse.ResponseStatus.OK, GetDownloadContent(sendTask), sendTask.File.FileType);
        }

        #endregion Public Methods

        #region Private Methods

        private byte[] GetDownloadContent(ISendTaskV2 sendTask) {
            if (sendTask.StorageFile == null) {
                return System.Text.Encoding.UTF8.GetBytes(sendTask.File.Preview ?? string.Empty);
            }

            return _storageHelper.ReadBytesFromFileAsync(sendTask.StorageFile).GetAwaiter().GetResult();
        }

        #endregion Private Methods

    }
}