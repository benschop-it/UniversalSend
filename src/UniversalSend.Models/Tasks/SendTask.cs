using System.ComponentModel;
using UniversalSend.Models.Data;
using Windows.Storage;

namespace UniversalSend.Models.Tasks {

    public class SendTask {

        #region Public Enums

        public enum ReceiveTaskStates {

            [Description("Waiting")]
            Waiting,

            [Description("Transferring")]
            Sending,

            [Description("Canceled")]
            Canceled,

            [Description("Error")]
            Error,

            [Description("Completed")]
            Done
        }

        #endregion Public Enums

        #region Public Properties

        public UniversalSendFile File { get; set; }

        public StorageFile StorageFile { get; set; }

        public ReceiveTaskStates TaskState { get; set; } = ReceiveTaskStates.Waiting;

        #endregion Public Properties

    }
}