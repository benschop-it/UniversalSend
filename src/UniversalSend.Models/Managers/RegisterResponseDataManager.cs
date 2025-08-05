using Newtonsoft.Json;
using System.Diagnostics;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Managers {

    internal class RegisterResponseDataManager : IRegisterResponseDataManager {

        #region Public Methods

        public IAnnouncementV1 DeserializeAnnouncementV1(string json) {
            var payload = JsonConvert.DeserializeObject<AnnouncementV1>(json);
            if (payload.Fingerprint == ProgramData.LocalDevice.Fingerprint) {
                Debug.WriteLine("Ignore self!");
                return null; // Ignore self
            }
            return payload;
        }

        public IAnnouncementV2 DeserializeAnnouncementV2(string json) {
            var payload = JsonConvert.DeserializeObject<AnnouncementV2>(json);
            if (payload.Fingerprint == ProgramData.LocalDevice.Fingerprint) {
                Debug.WriteLine("Ignore self!");
                return null; // Ignore self
            }
            return payload;
        }

        public IRegisterResponseDataV1 DeserializeRegisterResponseDataV1(string json) {
            var payload = JsonConvert.DeserializeObject<IRegisterResponseDataV1>(json);
            if (payload.Fingerprint == ProgramData.LocalDevice.Fingerprint) {
                Debug.WriteLine("Ignore self!");
                return null; // Ignore self
            }
            return payload;
        }

        public IRegisterResponseDataV2 DeserializeRegisterResponseDataV2(string json) {
            var payload = JsonConvert.DeserializeObject<IRegisterResponseDataV2>(json);
            if (payload.Fingerprint == ProgramData.LocalDevice.Fingerprint) {
                Debug.WriteLine("Ignore self!");
                return null; // Ignore self
            }
            return payload;
        }

        public IAnnouncementV1 GetAnnouncementV1(bool announcement) {
            var registerResponseData = new AnnouncementV1 {
                Alias = ProgramData.LocalDevice.Alias,
                DeviceModel = ProgramData.LocalDevice.DeviceModel,
                DeviceType = ProgramData.LocalDevice.DeviceType,
                Fingerprint = ProgramData.LocalDevice.Fingerprint,
                Announcement = announcement
            };
            return registerResponseData;
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
                Download = "false",
                Announce = announce
            };
            return registerResponseData;
        }

        public IRegisterResponseDataV1 GetRegisterResponseDataV1(bool announcement) {
            var registerResponseData = new RegisterResponseDataV1 {
                Alias = ProgramData.LocalDevice.Alias,
                DeviceModel = ProgramData.LocalDevice.DeviceModel,
                DeviceType = ProgramData.LocalDevice.DeviceType,
                Fingerprint = ProgramData.LocalDevice.Fingerprint
            };
            return registerResponseData;
        }

        public IRegisterResponseDataV2 GetRegisterResponseDataV2(bool announce) {
            var registerResponseData = new RegisterResponseDataV2 {
                Alias = ProgramData.LocalDevice.Alias,
                Version = "2.1",
                DeviceModel = ProgramData.LocalDevice.DeviceModel,
                DeviceType = ProgramData.LocalDevice.DeviceType,
                Fingerprint = ProgramData.LocalDevice.Fingerprint,
                Port = ProgramData.LocalDevice.Port,
                Protocol = "http",
                Download = "false"
            };
            return registerResponseData;
        }


        #endregion Public Methods

    }
}