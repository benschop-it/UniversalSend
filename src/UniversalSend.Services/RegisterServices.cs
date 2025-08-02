using Microsoft.Extensions.DependencyInjection;
using UniversalSend.Services.HttpMessage.Plumbing;
using UniversalSend.Services.HttpMessage.ServerRequestParsers;
using UniversalSend.Services.HttpMessage.ServerResponseParsers;
using UniversalSend.Services.InstanceCreators;
using UniversalSend.Services.Interfaces;
using UniversalSend.Services.Interfaces.Internal;
using UniversalSend.Services.Misc;

namespace UniversalSend.Services {

    public static class RegisterServices {

        #region Public Methods

        public static void Register(IServiceCollection services) {
            services.AddSingleton<IOperationFunctions, OperationFunctions>();
            services.AddSingleton<IHttpRequestParser, HttpRequestParser>();
            services.AddSingleton<IConfiguration, Configuration>();
            services.AddSingleton<IEncodingCache, EncodingCache>();
            services.AddSingleton<IHttpCodesTranslator, HttpCodesTranslator>();
            services.AddSingleton<IHttpServerResponseParser, HttpServerResponseParser>();
            services.AddSingleton<IServiceHttpServer, ServiceHttpServer>();
            services.AddSingleton<IInstanceCreatorCache, InstanceCreatorCache>();
        }

        #endregion Public Methods
    }
}