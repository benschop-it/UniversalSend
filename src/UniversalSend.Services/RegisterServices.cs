using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Plumbing;
using UniversalSend.Services.HttpMessage.ServerRequestParsers;
using UniversalSend.Services.HttpMessage.ServerResponseParsers;
using UniversalSend.Services.Interfaces;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services {
    public static class RegisterServices {

        public static void Register(IServiceCollection services) {
            services.AddSingleton<IOperationFunctions, OperationFunctions>();
            services.AddSingleton<IHttpRequestParser, HttpRequestParser>();
            services.AddSingleton<IConfiguration, Configuration>();
            services.AddSingleton<IEncodingCache, EncodingCache>();
            services.AddSingleton<IHttpCodesTranslator, HttpCodesTranslator>();
            services.AddSingleton<IHttpServerResponseParser, HttpServerResponseParser>();
            services.AddTransient<IServiceHttpServer, ServiceHttpServer>();
        }
    }
}
