using System;
using System.Collections.Generic;
using System.Reflection;
using UniversalSend.Services.Attributes;
using UniversalSend.Services.Interfaces.Internal;
using UniversalSend.Services.Models.Contracts;
using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services.InstanceCreators {

    internal class InstanceCreatorCache : IInstanceCreatorCache {

        #region Private Fields

        private Dictionary<Type, IInstanceCreator> _cache;

        #endregion Private Fields

        #region Internal Constructors

        public InstanceCreatorCache() {
            _cache = new Dictionary<Type, IInstanceCreator>();
        }

        #endregion Internal Constructors

        #region Internal Methods

        public void CacheCreator(Type restController) {
            if (!_cache.ContainsKey(restController)) {
                var restControllerAtt = restController.GetTypeInfo().GetCustomAttribute<RestControllerAttribute>();
                InstanceCreationType t = restControllerAtt != null ? restControllerAtt.InstanceCreationType : InstanceCreationType.Singleton;

                if (t == InstanceCreationType.PerCall) {
                    _cache[restController] = new PerCallInstanceCreator();
                } else {
                    _cache[restController] = new SingletonInstanceCreator();
                }
            }
        }

        public IInstanceCreator GetCreator(Type restController) {
            CacheCreator(restController);
            return _cache[restController];
        }

        #endregion Internal Methods
    }
}