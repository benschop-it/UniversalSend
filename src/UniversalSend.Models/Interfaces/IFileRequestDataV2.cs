using Newtonsoft.Json;

namespace UniversalSend.Models.Interfaces {
    public interface IFileRequestDataV2 {

        #region Public Properties

        string Id { get; set; }
        string FileName { get; set; }
        long Size { get; set; }
        string FileType { get; set; }
        string Sha256 { get; set; }
        string Preview { get; set; }
        MetaData Metadata { get; set; }

        #endregion Public Properties
    }

    public class MetaData {
        [JsonProperty("modified")]
        string Modified { get; set; }

        [JsonProperty("accessed")]
        string Accessed { get; set; }
    }
}