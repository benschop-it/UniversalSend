using UniversalSend.Models.Interfaces;
using Windows.Storage;

namespace UniversalSend.Models.Tasks {

    internal class SendTask : ISendTask {

        #region Public Properties

        public IUniversalSendFile File { get; set; }

        public IStorageFile StorageFile { get; set; }

        public ReceiveTaskStates TaskState { get; set; } = ReceiveTaskStates.Waiting;

        #endregion Public Properties

    }
}