namespace UniversalSend.Models.Interfaces {
    public interface IFileRequestData {

        #region Public Properties

        string FileName { get; set; }
        string FileType { get; set; }
        string Id { get; set; }
        string Preview { get; set; }
        long Size { get; set; }

        #endregion Public Properties
    }
}