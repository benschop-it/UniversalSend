using Newtonsoft.Json;
using System.Collections.Generic;

namespace UniversalSend.Models.HttpData {

    /// <summary>
    /// Dictionary that maps FileId to Token
    /// </summary>
    public sealed class FileResponseData : Dictionary<string, string> {
    }

    public sealed class FileResponseDataV2  {

        [JsonProperty("sessionid")]
        string SessionId { get; set; }

        [JsonProperty("files")]
        Dictionary<string, string> Files { get; set; }
    }
}