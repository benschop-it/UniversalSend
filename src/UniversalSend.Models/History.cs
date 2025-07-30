using System;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models {

    internal class History : IHistory {

        #region Public Constructors

        public History(IUniversalSendFile file, string futureAccessListToken, IDevice device) {
            File = file;
            FutureAccessListToken = futureAccessListToken;
            Device = device;
        }

        #endregion Public Constructors

        #region Public Properties

        public DateTime DateTime { get; set; } = DateTime.Now;

        public IDevice Device { get; set; }

        public IUniversalSendFile File { get; set; }

        public string FutureAccessListToken { get; set; }

        #endregion Public Properties
    }
}