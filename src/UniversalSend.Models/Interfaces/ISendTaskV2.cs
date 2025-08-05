using System.ComponentModel;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {

    public interface ISendTaskV2 {

        #region Public Properties

        IUniversalSendFileV2 File { get; set; }

        IStorageFile StorageFile { get; set; }

        ReceiveTaskStates TaskState { get; set; }
        string SessionId { get; set; }

        #endregion Public Properties
    }
}