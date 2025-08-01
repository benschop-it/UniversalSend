using Newtonsoft.Json;
using System;
using System.Diagnostics;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Attributes;
using UniversalSend.Services.Models.Schemas;
using Windows.Storage;

namespace UniversalSend.Services.Controllers {

    [RestController(InstanceCreationType.PerCall)]
    internal class V2RequestController {

        #region Private Fields

        private IRegisterDataManager _registerDataManager;
        private IStorageHelper _storageHelper;

        #endregion Private Fields

        #region Public Constructors

        public V2RequestController(IRegisterDataManager registerDataManager, IStorageHelper storageHelper) {
            _registerDataManager = registerDataManager ?? throw new ArgumentNullException(nameof(registerDataManager));
            _storageHelper = storageHelper ?? throw new ArgumentNullException(nameof(storageHelper));
        }

        #endregion Public Constructors

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

        #region Public Methods

        [UriFormat("v2/register")]
        public PostResponse PostRegister([FromContent] IRegisterData data) {
            Debug.WriteLine("POST /register called"); // Debug output
            if (data != null)
                Debug.WriteLine(JsonConvert.SerializeObject(data));
            return new PostResponse(
                PostResponse.ResponseStatus.Created,
                "", JsonConvert.SerializeObject(_registerDataManager.GetRegisterDataFromDevice()/*ProgramData.LocalDeviceRegisterData*/)); // Fix: pass string directly instead of anonymous type
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

        public async void SaveFileData(string fileId, byte[] data) {
            StorageFile storageFile = await _storageHelper.CreateFileInAppLocalFolderAsync(fileId);
            await _storageHelper.WriteBytesToFileAsync(storageFile, data);
        }

        #endregion Public Methods

    }
}