using System.ComponentModel;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;

namespace UniversalSend.Models.Interfaces {
    #region Public Enums

    public enum ReceiveTaskStates {

        [Description("Waiting")]
        Waiting,

        [Description("Transferring")]
        Sending,

        [Description("Transferring")]
        Receiving,

        [Description("Canceled")]
        Canceled,

        [Description("Error")]
        Error,

        [Description("Completed")]
        Done
    }

    #endregion Public Enums

    public interface IReceiveTask {
        IUniversalSendFile File { get; set; }
        byte[] FileContent { get; set; }
        IInfoData Sender { get; set; }
        ReceiveTaskStates TaskState { get; set; }
    }
}