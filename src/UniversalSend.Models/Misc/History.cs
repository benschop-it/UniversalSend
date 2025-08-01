using System;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Misc {

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

    internal class ConcreteHistory {

        #region Public Constructors

        public ConcreteHistory(UniversalSendFile file, string futureAccessListToken, Device device) {
            File = file;
            FutureAccessListToken = futureAccessListToken;
            Device = device;
        }

        #endregion Public Constructors

        #region Public Properties

        public DateTime DateTime { get; set; } = DateTime.Now;

        public Device Device { get; set; }

        public UniversalSendFile File { get; set; }

        public string FutureAccessListToken { get; set; }

        #endregion Public Properties
    }
}