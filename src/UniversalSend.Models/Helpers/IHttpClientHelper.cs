using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage.Streams;

namespace UniversalSend.Models.Helpers {
    public interface IHttpClientHelper {
        Task<bool> DeleteAsync(string url, Dictionary<string, string> headers = null);
        Task<JsonArray> GetJsonArrayAsync(string url, Dictionary<string, string> headers = null);
        Task<JsonObject> GetJsonObjectAsync(string url, Dictionary<string, string> headers = null);
        Task<IRandomAccessStream> GetStreamAsync(string url, Dictionary<string, string> headers = null);
        Task<string> GetStringAsync(string url, Dictionary<string, string> headers = null);
        Task<string> PostBinaryAsync(string url, byte[] binaryData, string contentType = "application/octet-stream", Dictionary<string, string> headers = null);
        Task<string> PostFormAsync(string url, Dictionary<string, string> formData, Dictionary<string, string> headers = null);
        Task<string> PostJsonAsync(string url, string jsonContent, Dictionary<string, string> headers = null);
        Task<string> PostStringAsync(string url, HttpContent content, Dictionary<string, string> headers = null);
        Task<string> PutJsonAsync(string url, string jsonContent, Dictionary<string, string> headers = null);
        Task<string> PutStringAsync(string url, HttpContent content, Dictionary<string, string> headers = null);
        void SetBaseAddress(string baseAddress);
        void SetTimeout(int seconds);
    }
}