using System.Collections.Generic;

namespace UniversalSend.Models.Interfaces {

    public interface IWebSendManager {

        bool BeginShare(IEnumerable<ISendTaskV2> sendTasks);

        void ClearShare(string sessionId = null);

        IWebSendSession GetActiveShare();
    }

    public interface IWebSendSession {

        IReadOnlyDictionary<string, ISendTaskV2> Files { get; }

        string SessionId { get; }
    }
}
