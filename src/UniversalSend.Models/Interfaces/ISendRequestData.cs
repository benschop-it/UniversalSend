using System.Collections.Generic;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces {

    public interface ISendRequestData {

        #region Public Properties

        Dictionary<string, FileRequestData> Files { get; set; }
        InfoData Info { get; set; }

        #endregion Public Properties
    }
}