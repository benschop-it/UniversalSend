using System.Collections.Generic;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces {

    public interface ISendRequestDataV1 {

        #region Public Properties

        Dictionary<string, FileRequestDataV1> Files { get; set; }
        InfoDataV1 Info { get; set; }

        #endregion Public Properties
    }
}