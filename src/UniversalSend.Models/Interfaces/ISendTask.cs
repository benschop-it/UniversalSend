using System.ComponentModel;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {

    public interface ISendTask {

        #region Public Properties

        IUniversalSendFile File { get; set; }

        IStorageFile StorageFile { get; set; }

        ReceiveTaskStates TaskState { get; set; }

        #endregion Public Properties
    }
}