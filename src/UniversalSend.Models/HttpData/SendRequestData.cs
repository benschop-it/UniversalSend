using Newtonsoft.Json;
using System.Collections.Generic;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    public sealed class SendRequestData : ISendRequestData {

        #region Public Properties

        [JsonProperty("files")]
        public Dictionary<string, FileRequestData> Files { get; set; }

        [JsonProperty("info")]
        public InfoData Info { get; set; }

        #endregion Public Properties
    }
}