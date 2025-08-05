using System.Collections.Generic;
using UniversalSend.Models.HttpData;

namespace UniversalSend.Models.Interfaces {

    public interface ISendRequestDataV2 {

        #region Public Properties

        Dictionary<string, FileRequestDataV2> Files { get; set; }
        InfoDataV2 Info { get; set; }

        #endregion Public Properties
    }
}