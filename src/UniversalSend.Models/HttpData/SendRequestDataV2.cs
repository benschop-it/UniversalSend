using Newtonsoft.Json;
using System.Collections.Generic;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    public sealed class SendRequestDataV2 : ISendRequestDataV2 {

        #region Public Properties

        [JsonProperty("files")]
        public Dictionary<string, FileRequestDataV2> Files { get; set; }

        [JsonProperty("info")]
        public InfoDataV2 Info { get; set; }

        #endregion Public Properties

    }
}