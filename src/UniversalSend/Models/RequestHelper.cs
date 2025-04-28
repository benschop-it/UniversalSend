using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http.Headers;
using Windows.Web.Http;

namespace UniversalSend.Models
{
    public class RequestHelper
    {
        public async Task<string> PostDataWithWindowsWebHttp(string ipAddress, int port, string endpoint, string jsonData)
        {
            try
            {
                var httpClient = new HttpClient();

                // 设置请求头
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new HttpMediaTypeWithQualityHeaderValue("application/json"));

                Uri requestUri = new Uri($"http://{ipAddress}:{port}/{endpoint}");
                HttpStringContent content = new HttpStringContent(
                    jsonData,
                    Windows.Storage.Streams.UnicodeEncoding.Utf8,
                    "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
