using System.ComponentModel;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Tasks {

    internal class ReceiveTask : IReceiveTask {

        #region Public Properties

        public IUniversalSendFile File { get; set; }

        public byte[] FileContent { get; set; }

        public IInfoData Sender { get; set; }

        public ReceiveTaskStates TaskState { get; set; } = ReceiveTaskStates.Waiting;

        #endregion Public Properties
    }
}