using UniversalSend.Services.Models.Contracts;

namespace UniversalSend.Services.Rest
{
    internal class RestControllerMethodExecutorFactory
    {
        private IRestMethodExecutor _withoutContentExecutor;
        private IRestMethodExecutor _withContentExecutor;

        public RestControllerMethodExecutorFactory()
        {
            _withoutContentExecutor = new RestControllerMethodExecutor();
            _withContentExecutor = new RestControllerMethodWithContentExecutor();

        }
        internal IRestMethodExecutor Create(RestControllerMethodInfo info)
        {
            if (info.HasContentParameter)
            {
                return _withContentExecutor;
            }
            else
            {
                return _withoutContentExecutor;
            }
        }
    }
}
