using Newtonsoft.Json;
using System.Diagnostics;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {
    internal class RegisterResponseDataManager : IRegisterResponseDataManager {
        public IRegisterResponseData DeserializeRegisterResponseData(string json) {
            var payload = JsonConvert.DeserializeObject<RegisterResponseData>(json);
            if (payload.Fingerprint == ProgramData.LocalDevice.Fingerprint) {
                Debug.WriteLine("Ignore self!");
                return null; // Ignore self
            }
            return payload;
        }

        #region Public Methods

        public IRegisterResponseData GetRegisterReponseData(bool announcement) {
            var registerResponseData = new RegisterResponseData {
                Alias = ProgramData.LocalDevice.Alias,
                DeviceModel = ProgramData.LocalDevice.DeviceModel,
                DeviceType = ProgramData.LocalDevice.DeviceType,
                Fingerprint = ProgramData.LocalDevice.Fingerprint,
                Announcement = announcement
            };
            return registerResponseData;
        }

        #endregion Public Methods
    }
}