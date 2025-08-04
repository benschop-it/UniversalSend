using System.ComponentModel;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Tasks;
using UniversalSend.Services.Interfaces;

namespace UniversalSend.Models.Interfaces {

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

    public interface IReceiveTask: INotifyPropertyChanged {

        #region Public Properties

        IUniversalSendFile File { get; set; }
        byte[] FileContent { get; set; }
        IInfoData Sender { get; set; }
        ReceiveTaskStates TaskState { get; set; }
        int Progress { get; set; }
        HttpParseProgressStatus Status { get; set; }
        string Error { get; set; }


        #endregion Public Properties

        void Refresh(string propertyName);
    }
}