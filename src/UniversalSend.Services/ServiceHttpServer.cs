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
            restRouteHandler.RegisterController<V1RequestController>(); // Register controller
            //restRouteHandler.RegisterController<V2RequestController>(); // Register controller
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

            // Output server address

            // Plugin functionality
            // File transfer
            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v1/send?fileId={}&token={}"))
                OperationController.UriOperations.Add("/api/localsend/v1/send?fileId={}&token={}", OperationFunctions.SendRequestFuncAsync);
            if (!OperationController.UriOperations.ContainsKey("/api/localsend/v1/register"))
                OperationController.UriOperations.Add("/api/localsend/v1/register", OperationFunctions.RegisterRequestFunc);

            Debug.WriteLine($"HTTP server started on port {port}");
            return isRunning = true;
        }

        public void StopHttpServer()
        {
            httpServer.StopServer();
            isRunning = false;
            Debug.WriteLine($"HTTP server has stopped");
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
