using Newtonsoft.Json;
using System.Collections.Generic;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    public sealed class SendRequestDataV1 : ISendRequestDataV1 {

        #region Public Properties

        [JsonProperty("files")]
        public Dictionary<string, FileRequestDataV1> Files { get; set; }

        [JsonProperty("info")]
        public InfoDataV1 Info { get; set; }

        #endregion Public Properties

    }
}