using System.Collections.Generic;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.HttpData {

    public sealed class DownloadPrepareResponseData {

        public Dictionary<string, IFileRequestDataV2> Files { get; set; } = new Dictionary<string, IFileRequestDataV2>();

        public InfoDataV2 Info { get; set; }

        public string SessionId { get; set; }
    }
}
