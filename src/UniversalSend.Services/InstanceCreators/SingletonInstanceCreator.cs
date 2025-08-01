using System;
using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.InstanceCreators {

    internal class SingletonInstanceCreator : IInstanceCreator {

        #region Private Fields

        private object _instance;
        private object _instanceLock = new object();

        #endregion Private Fields

        #region Public Methods

        public object Create(Type instanceType, params object[] args) {
            CacheInstance(instanceType, args);

            return _instance;
        }

        #endregion Public Methods

        #region Private Methods

        private void CacheInstance(Type instanceType, object[] args) {
            if (_instance == null) {
                lock (_instanceLock) {
                    if (_instance == null) {
                        _instance = Activator.CreateInstance(instanceType, args);
                    }
                }
            }
        }

        #endregion Private Methods
    }
}