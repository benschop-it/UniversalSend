using UniversalSend.Services.Interfaces;

namespace UniversalSend.Models.Interfaces {
    public interface IUniversalSendFileV2 {

        #region Public Properties

        string Id { get; set; }
        string FileName { get; set; }
        long Size { get; set; }
        string FileType { get; set; }
        string Sha256 { get; set; }
        string Preview { get; set; }
        string TransferToken { get; set; }

        #endregion Public Properties
    }
}