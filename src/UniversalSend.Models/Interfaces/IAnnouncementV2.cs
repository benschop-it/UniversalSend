using Newtonsoft.Json;

namespace UniversalSend.Models.Interfaces {

    public interface IAnnouncementV2 : IRegisterResponseDataV2 {

        #region Public Properties

        bool Announce { get; set; }

        #endregion Public Properties
    }
}