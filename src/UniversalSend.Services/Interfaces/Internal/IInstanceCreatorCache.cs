using System;
using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.Interfaces.Internal {
    internal interface IInstanceCreatorCache {
        void CacheCreator(Type restController);
        IInstanceCreator GetCreator(Type restController);
    }
}