using UniversalSend.Services.Interfaces.Internal;
using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.Rest {

    internal class RestControllerMethodExecutorFactory {

        #region Private Fields

        private IRestMethodExecutor _withContentExecutor;
        private IRestMethodExecutor _withoutContentExecutor;

        #endregion Private Fields

        #region Public Constructors

        public RestControllerMethodExecutorFactory(IInstanceCreatorCache instanceCreatorCache) {
            _withoutContentExecutor = new RestControllerMethodExecutor(instanceCreatorCache);
            _withContentExecutor = new RestControllerMethodWithContentExecutor(instanceCreatorCache);
        }

        #endregion Public Constructors

        #region Internal Methods

        internal IRestMethodExecutor Create(RestControllerMethodInfo info) {
            if (info.HasContentParameter) {
                return _withContentExecutor;
            } else {
                return _withoutContentExecutor;
            }
        }

        #endregion Internal Methods
    }
}