using Newtonsoft.Json;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    internal class AnnouncementV2 : RegisterResponseDataV2, IAnnouncementV2 {

        #region Public Properties

        [JsonProperty("announce")]
        public bool Announce { get; set; }

        #endregion Public Properties

    }
}