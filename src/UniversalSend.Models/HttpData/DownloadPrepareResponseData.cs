using Newtonsoft.Json;
using System.Collections.Generic;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    public sealed class DownloadPrepareResponseData {

        [JsonProperty("files")]
        public Dictionary<string, IFileRequestDataV2> Files { get; set; } = new Dictionary<string, IFileRequestDataV2>();

        [JsonProperty("info")]
        public InfoDataV2 Info { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }
    }
}
