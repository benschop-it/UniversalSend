using System.ComponentModel;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {

    public interface ISendTaskV1 {

        #region Public Properties

        IUniversalSendFileV1 File { get; set; }

        IStorageFile StorageFile { get; set; }

        ReceiveTaskStates TaskState { get; set; }

        #endregion Public Properties
    }
}