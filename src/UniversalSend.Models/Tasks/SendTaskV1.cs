using UniversalSend.Models.Interfaces;
using Windows.Storage;

namespace UniversalSend.Models.Tasks {

    internal class SendTaskV1 : ISendTaskV1 {

        #region Public Properties

        public IUniversalSendFileV1 File { get; set; }

        public IStorageFile StorageFile { get; set; }

        public ReceiveTaskStates TaskState { get; set; } = ReceiveTaskStates.Waiting;

        #endregion Public Properties

    }
}