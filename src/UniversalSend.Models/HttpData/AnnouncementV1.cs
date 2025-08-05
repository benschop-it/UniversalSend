using Newtonsoft.Json;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    internal class AnnouncementV1 : RegisterResponseDataV1, IAnnouncementV1 {

        #region Public Properties

        [JsonProperty("announcement")]
        public bool Announcement { get; set; }

        #endregion Public Properties

    }
}