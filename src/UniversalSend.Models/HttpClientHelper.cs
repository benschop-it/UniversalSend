﻿using System;
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
        // Static HttpClient instance to avoid frequent creation
        private static readonly HttpClient _httpClient = new HttpClient();

        static HttpClientHelper()
        {
            // Set default timeout
            //_httpClient.Timeout = TimeSpan.FromSeconds(30);
            // Set default request headers
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region GET requests
        /// <summary>
        /// Send a GET request and return string result
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
                // Handle exception, customize handling logic if needed
                System.Diagnostics.Debug.WriteLine($"GET request failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Send a GET request and return JsonArray result
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
        /// Send a GET request and return JsonObject result
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
        /// Send a GET request and return stream result
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
                System.Diagnostics.Debug.WriteLine($"GET stream request failed: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region POST requests
        /// <summary>
        /// Send a POST request and return string result
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
                System.Diagnostics.Debug.WriteLine($"POST request failed: {ex.Message}");
                return "";
            }
        }

        public static async Task<string> PostBinaryAsync(string url, byte[] binaryData, string contentType = "application/octet-stream", Dictionary<string, string> headers = null)
        {
            var content = new ByteArrayContent(binaryData);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return await PostStringAsync(url, content, headers);
        }

        /// <summary>
        /// Send a JSON POST request and return string result
        /// </summary>
        public static async Task<string> PostJsonAsync(string url, string jsonContent, Dictionary<string, string> headers = null)
        {
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await PostStringAsync(url, content, headers);
        }

        /// <summary>
        /// Send a form POST request and return string result
        /// </summary>
        public static async Task<string> PostFormAsync(string url, Dictionary<string, string> formData, Dictionary<string, string> headers = null)
        {
            var content = new FormUrlEncodedContent(formData);
            return await PostStringAsync(url, content, headers);
        }
        #endregion

        #region PUT requests
        /// <summary>
        /// Send a PUT request and return string result
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
                System.Diagnostics.Debug.WriteLine($"PUT request failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Send a JSON PUT request and return string result
        /// </summary>
        public static async Task<string> PutJsonAsync(string url, string jsonContent, Dictionary<string, string> headers = null)
        {
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await PutStringAsync(url, content, headers);
        }
        #endregion

        #region DELETE requests
        /// <summary>
        /// Send a DELETE request
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
                System.Diagnostics.Debug.WriteLine($"DELETE request failed: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Apply custom headers
        /// </summary>
        private static void ApplyHeaders(Dictionary<string, string> headers)
        {
            // Clear previous custom headers
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
        /// Set base URL
        /// </summary>
        public static void SetBaseAddress(string baseAddress)
        {
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        /// <summary>
        /// Set timeout (in seconds)
        /// </summary>
        public static void SetTimeout(int seconds)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(seconds);
        }
        #endregion
    }
}
