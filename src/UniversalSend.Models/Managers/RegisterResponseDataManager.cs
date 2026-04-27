using Newtonsoft.Json;
using UniversalSend.Models.Common;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    internal class RegisterResponseDataManager : IRegisterResponseDataManager {

        private readonly ILogger _logger = LogManager.GetLogger<RegisterResponseDataManager>();

        #region Public Methods

        public IAnnouncementV2 DeserializeAnnouncementV2(string json) {
            var payload = JsonConvert.DeserializeObject<AnnouncementV2>(json);
            if (payload.Fingerprint == ProgramData.LocalDevice.Fingerprint) {
                return null; // Ignore self
            }
            return payload;
        }

        public IRegisterResponseDataV2 DeserializeRegisterResponseDataV2(string json) {
            var payload = JsonConvert.DeserializeObject<IRegisterResponseDataV2>(json);
            if (payload.Fingerprint == ProgramData.LocalDevice.Fingerprint) {
                return null; // Ignore self
            }
            return payload;
        }

        public IAnnouncementV2 GetAnnouncementV2(bool announce) {
            var registerResponseData = new AnnouncementV2 {
                Alias = ProgramData.LocalDevice.Alias,
                Version = "2.1",
                DeviceModel = ProgramData.LocalDevice.DeviceModel,
                DeviceType = ProgramData.LocalDevice.DeviceType,
                Fingerprint = ProgramData.LocalDevice.Fingerprint,
                Port = ProgramData.LocalDevice.Port,
                Protocol = "http",
                Download = false,
                Announce = announce
            };
            return registerResponseData;
        }

        public IRegisterResponseDataV2 GetRegisterResponseDataV2() {
            var registerResponseData = new RegisterResponseDataV2 {
                Alias = ProgramData.LocalDevice.Alias,
                Version = "2.1",
                DeviceModel = ProgramData.LocalDevice.DeviceModel,
                DeviceType = ProgramData.LocalDevice.DeviceType,
                Fingerprint = ProgramData.LocalDevice.Fingerprint,
                //Port = ProgramData.LocalDevice.Port,
                //Protocol = "http",
                Download = false
            };
            return registerResponseData;
        }


        #endregion Public Methods

    }
}