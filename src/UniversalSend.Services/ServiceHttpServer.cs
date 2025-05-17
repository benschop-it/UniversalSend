using Restup.Webserver.Http;
using Restup.Webserver.Rest;
using Restup.WebServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace UniversalSend.Services
{
    public class ServiceHttpServer
    {
        public HttpServerConfiguration httpServerConfiguration { get; set; }

        public HttpServer httpServer { get; set; }

        bool isRunning = false;

        public async Task<bool> StartHttpServerAsync(int port)
        {
            RestRouteHandler restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<V1RequestController>(); // 注册控制器
            //restRouteHandler.RegisterController<V2RequestController>(); // 注册控制器
            httpServerConfiguration = new HttpServerConfiguration();
            httpServerConfiguration.ListenOnPort(port).RegisterRoute("api/localsend/", restRouteHandler).EnableCors();
            
            httpServer = new HttpServer(httpServerConfiguration);
            try
            {
                await httpServer.StartServerAsync();
            }
            catch
            {
                return false;
            }
            //httpServer._listener.ConnectionReceived += _listener_ConnectionReceived;
            // 输出服务器地址


            //外挂功能
            //文件传输
            if(OperationController.UriOperations.ContainsKey("/api/localsend/v1/send?fileId={}&token={}") == false)
                OperationController.UriOperations.Add("/api/localsend/v1/send?fileId={}&token={}",OperationFunctions.SendRequestFuncAsync);
            if (OperationController.UriOperations.ContainsKey("/api/localsend/v1/register") == false)
                OperationController.UriOperations.Add("/api/localsend/v1/register",OperationFunctions.RegisterRequestFunc);


            Debug.WriteLine($"HTTP 服务器已在{port}端口上启动");
            return isRunning = true;
        }

        public void StopHttpServer()
        {
            httpServer.StopServer();
            isRunning = false;
            Debug.WriteLine($"HTTP 服务器已停止");
        }

        //private void _listener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        //{
        //    Debug.WriteLine(args.Socket.InputStream);
        //    var inputStream = args.Socket.InputStream;
        //    using (var dataReader = new DataReader(inputStream))
        //    {
        //        byte[] Byte_data = new byte[dataReader.UnconsumedBufferLength];
        //        dataReader.ReadBytes(Byte_data);
        //        Debug.WriteLine(Byte_data.ToString());
        //    }
        //}
    }
}
