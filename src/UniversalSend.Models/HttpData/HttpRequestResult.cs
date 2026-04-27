namespace UniversalSend.Models.HttpData {

    internal sealed class HttpRequestResult {

        #region Public Properties

        public string Content { get; set; } = string.Empty;

        public bool IsSuccessStatusCode => StatusCode >= 200 && StatusCode <= 299;

        public int StatusCode { get; set; }

        #endregion Public Properties
    }
}
