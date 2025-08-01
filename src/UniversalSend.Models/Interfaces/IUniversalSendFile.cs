namespace UniversalSend.Models.Interfaces {
    public interface IUniversalSendFile {

        #region Public Properties

        string FileName { get; set; }
        string FileType { get; set; }
        string Id { get; set; }
        long Size { get; set; }
        string Text { get; set; }
        string TransferToken { get; set; }

        #endregion Public Properties
    }
}