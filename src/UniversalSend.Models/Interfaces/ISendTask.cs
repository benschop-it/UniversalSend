using System.ComponentModel;
using Windows.Storage;

namespace UniversalSend.Models.Interfaces {

    public interface ISendTask {
        IUniversalSendFile File { get; set; }

        IStorageFile StorageFile { get; set; }

        ReceiveTaskStates TaskState { get; set; }
    }
}