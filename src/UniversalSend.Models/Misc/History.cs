using System;
using UniversalSend.Models.Data;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Misc {

    internal class ConcreteHistory {

        #region Public Constructors

        public ConcreteHistory(UniversalSendFileV1 file, string futureAccessListToken, Device device) {
            File = file;
            FutureAccessListToken = futureAccessListToken;
            Device = device;
        }

        #endregion Public Constructors

        #region Public Properties

        public DateTime DateTime { get; set; } = DateTime.Now;

        public Device Device { get; set; }

        public UniversalSendFileV1 File { get; set; }

        public string FutureAccessListToken { get; set; }

        #endregion Public Properties

    }

    internal class History : IHistory {

        #region Public Constructors

        public History(IUniversalSendFileV1 file, string futureAccessListToken, IDevice device) {
            File = file;
            FutureAccessListToken = futureAccessListToken;
            Device = device;
        }

        #endregion Public Constructors

        #region Public Properties

        public DateTime DateTime { get; set; } = DateTime.Now;

        public IDevice Device { get; set; }

        public IUniversalSendFileV1 File { get; set; }

        public string FutureAccessListToken { get; set; }

        #endregion Public Properties

    }
}