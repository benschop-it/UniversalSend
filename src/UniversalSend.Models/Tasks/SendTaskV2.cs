using UniversalSend.Models.Interfaces;
using Windows.Storage;

namespace UniversalSend.Models.Tasks {

    internal class SendTaskV2 : ISendTaskV2 {

        #region Public Properties

        public IUniversalSendFileV2 File { get; set; }

        public IStorageFile StorageFile { get; set; }

        public ReceiveTaskStates TaskState { get; set; } = ReceiveTaskStates.Waiting;

        public string SessionId { get; set; }

        #endregion Public Properties

    }
}