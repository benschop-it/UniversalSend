using System;
using System.Collections.Generic;
using System.Reflection;
using UniversalSend.Services.Attributes;
using UniversalSend.Services.Models.Contracts;
using UniversalSend.Services.Models.Schemas;

namespace UniversalSend.Services.InstanceCreators {

    internal class InstanceCreatorCache {

        #region Private Fields

        private Dictionary<Type, IInstanceCreator> _cache;

        #endregion Private Fields

        #region Public Constructors

        static InstanceCreatorCache() {
            Default = new InstanceCreatorCache();
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal InstanceCreatorCache() {
            _cache = new Dictionary<Type, IInstanceCreator>();
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal static InstanceCreatorCache Default { get; }

        #endregion Internal Properties

        #region Internal Methods

        internal void CacheCreator(Type restController) {
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

        internal IInstanceCreator GetCreator(Type restController) {
            CacheCreator(restController);
            return _cache[restController];
        }

        #endregion Internal Methods
    }
}