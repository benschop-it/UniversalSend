using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage.Streams;
using System.IO;

namespace UniversalSend.Models
{
    public static class HttpClientHelper
    {
        // 静态HttpClient实例，避免频繁创建
        private static readonly HttpClient _httpClient = new HttpClient();

        static HttpClientHelper()
        {
            // 设置默认超时时间
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            // 设置默认请求头
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region GET请求
        /// <summary>
        /// 发送GET请求并返回字符串结果
        /// </summary>
        public static async Task<string> GetStringAsync(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                ApplyHeaders(headers);
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // 处理异常，可根据需要自定义处理逻辑
                System.Diagnostics.Debug.WriteLine($"GET请求失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 发送GET请求并返回JsonArray结果
        /// </summary>
        public static async Task<JsonArray> GetJsonArrayAsync(string url, Dictionary<string, string> headers = null)
        {
            var jsonString = await GetStringAsync(url, headers);
            if (JsonArray.TryParse(jsonString, out JsonArray jsonArray))
            {
                return jsonArray;
            }
            return new JsonArray();
        }

        /// <summary>
        /// 发送GET请求并返回JsonObject结果
        /// </summary>
        public static async Task<JsonObject> GetJsonObjectAsync(string url, Dictionary<string, string> headers = null)
        {
            var jsonString = await GetStringAsync(url, headers);
            if (JsonObject.TryParse(jsonString, out JsonObject jsonObject))
            {
                return jsonObject;
            }
            return new JsonObject();
        }

        /// <summary>
        /// 发送GET请求并返回流结果
        /// </summary>
        public static async Task<IRandomAccessStream> GetStreamAsync(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                ApplyHeaders(headers);
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                return stream.AsRandomAccessStream();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GET流请求失败: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region POST请求
        /// <summary>
        /// 发送POST请求并返回字符串结果
        /// </summary>
        public static async Task<string> PostStringAsync(string url, HttpContent content, Dictionary<string, string> headers = null)
        {
            try
            {
                ApplyHeaders(headers);
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"POST请求失败: {ex.Message}");
                return "403 Forbidden";
            }
        }

        public static async Task<string> PostBinaryAsync(string url, byte[] binaryData, string contentType = "application/octet-stream", Dictionary<string, string> headers = null)
        {
            var content = new ByteArrayContent(binaryData);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return await PostStringAsync(url, content, headers);
        }

        /// <summary>
        /// 发送JSON POST请求并返回字符串结果
        /// </summary>
        public static async Task<string> PostJsonAsync(string url, string jsonContent, Dictionary<string, string> headers = null)
        {
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await PostStringAsync(url, content, headers);
        }

        /// <summary>
        /// 发送表单POST请求并返回字符串结果
        /// </summary>
        public static async Task<string> PostFormAsync(string url, Dictionary<string, string> formData, Dictionary<string, string> headers = null)
        {
            var content = new FormUrlEncodedContent(formData);
            return await PostStringAsync(url, content, headers);
        }
        #endregion

        #region PUT请求
        /// <summary>
        /// 发送PUT请求并返回字符串结果
        /// </summary>
        public static async Task<string> PutStringAsync(string url, HttpContent content, Dictionary<string, string> headers = null)
        {
            try
            {
                ApplyHeaders(headers);
                var response = await _httpClient.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PUT请求失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 发送JSON PUT请求并返回字符串结果
        /// </summary>
        public static async Task<string> PutJsonAsync(string url, string jsonContent, Dictionary<string, string> headers = null)
        {
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await PutStringAsync(url, content, headers);
        }
        #endregion

        #region DELETE请求
        /// <summary>
        /// 发送DELETE请求
        /// </summary>
        public static async Task<bool> DeleteAsync(string url, Dictionary<string, string> headers = null)
        {
            try
            {
                ApplyHeaders(headers);
                var response = await _httpClient.DeleteAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DELETE请求失败: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 应用自定义请求头
        /// </summary>
        private static void ApplyHeaders(Dictionary<string, string> headers)
        {
            // 清除之前的自定义头
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// 设置基础URL
        /// </summary>
        public static void SetBaseAddress(string baseAddress)
        {
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        /// <summary>
        /// 设置超时时间（秒）
        /// </summary>
        public static void SetTimeout(int seconds)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(seconds);
        }
        #endregion
    }
}
