using System;
using UniversalSend.Models.Data;

namespace UniversalSend.Models {

    public class History {

        #region Public Constructors

        public History(UniversalSendFile file, string futureAccessListToken, Device device) {
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