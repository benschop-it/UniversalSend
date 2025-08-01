using System;

namespace UniversalSend.Models.Interfaces {
    public interface IHistory {

        #region Public Properties

        DateTime DateTime { get; set; }
        IDevice Device { get; set; }
        IUniversalSendFile File { get; set; }
        string FutureAccessListToken { get; set; }

        #endregion Public Properties
    }
}