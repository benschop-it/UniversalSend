using System;
using System.Threading.Tasks;
using UniversalSend.Services.Models.Contracts;
using UniversalSend.Services.Models.Schemas;
using UniversalSend.Services.Rest.Models.Contracts;
using Windows.Foundation;

namespace UniversalSend.Services.Rest {

    internal abstract class RestMethodExecutor : IRestMethodExecutor {

        #region Public Methods

        public async Task<IRestResponse> ExecuteMethodAsync(RestControllerMethodInfo info, RestServerRequest request, ParsedUri requestUri) {
            var methodInvokeResult = ExecuteAnonymousMethod(info, request, requestUri);
            switch (info.ReturnTypeWrapper) {
                case RestControllerMethodInfo.TypeWrapper.None:
                    return await Task.FromResult((IRestResponse)methodInvokeResult);
                case RestControllerMethodInfo.TypeWrapper.AsyncOperation:
                    return await ConvertToTask((dynamic)methodInvokeResult);
                case RestControllerMethodInfo.TypeWrapper.Task:
                    return await (dynamic)methodInvokeResult;
            }

            throw new Exception($"ReturnTypeWrapper of type {info.ReturnTypeWrapper} not known.");
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract object ExecuteAnonymousMethod(RestControllerMethodInfo info, RestServerRequest request, ParsedUri requestUri);

        #endregion Protected Methods

        #region Private Methods

        private static Task<T> ConvertToTask<T>(IAsyncOperation<T> methodInvokeResult) {
            return methodInvokeResult.AsTask();
        }

        #endregion Private Methods
    }
}