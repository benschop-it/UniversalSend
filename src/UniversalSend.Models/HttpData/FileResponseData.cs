using Newtonsoft.Json;
using System.Collections.Generic;

namespace UniversalSend.Models.HttpData {

    public sealed class FileResponseDataV2  {

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        [JsonProperty("files")]
        public Dictionary<string, string> Files { get; set; } = new Dictionary<string, string>();
    }
}