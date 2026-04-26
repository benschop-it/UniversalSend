using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Headers.Request;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.HttpMessage.ServerRequestParsers;
using UniversalSend.Services.Interfaces.Internal;
using Windows.Storage.Streams;

namespace UniversalSend.Services.HttpMessage {

    internal class MutableHttpServerRequest : IHttpServerRequest, IMutableHttpServerRequest {

        #region Private Fields

        private readonly List<IHttpRequestHeader> _headers;
        private readonly HttpRequestParser _httpRequestParser;

        #endregion Private Fields

        #region Internal Constructors

        internal MutableHttpServerRequest(HttpRequestParser httpRequestParser) {
            _headers = new List<IHttpRequestHeader>();
            _httpRequestParser = httpRequestParser;

            AcceptCharsets = Enumerable.Empty<string>();
            AcceptMediaTypes = Enumerable.Empty<string>();
            AcceptEncodings = Enumerable.Empty<string>();
            AccessControlRequestHeaders = Enumerable.Empty<string>();
        }

        #endregion Internal Constructors

        #region Public Properties

        public IEnumerable<string> AcceptCharsets { get; set; }
        public IEnumerable<string> AcceptEncodings { get; set; }
        public IEnumerable<string> AcceptMediaTypes { get; set; }
        public IEnumerable<string> AccessControlRequestHeaders { get; set; }
        public HttpMethod? AccessControlRequestMethod { get; set; }
        public byte[] Content { get; set; }
        public int ContentLength { get; set; }
        public string ContentType { get; set; }
        public string ContentTypeCharset { get; set; }
        public IEnumerable<IHttpRequestHeader> Headers => _headers;
        public string HttpVersion { get; set; }
        public bool IsComplete { get; set; }
        public HttpMethod? Method { get; set; }
        public string Origin { get; set; }
        public string RemoteAddress { get; set; }
        public int? RemotePort { get; set; }
        public Uri Uri { get; set; }

        #endregion Public Properties

        public override string ToString() {
            var nl = Environment.NewLine;
            var sb = new StringBuilder(512);

            sb.Append("MutableHttpServerRequest").Append(nl)
              .Append("  Method: ").Append(Method != null ? Method.ToString() : "<null>").Append(nl)
              .Append("  Uri: ").Append(Uri != null ? Uri.ToString() : "<null>").Append(nl)
              .Append("  HttpVersion: ").Append(HttpVersion ?? "<null>").Append(nl)
              .Append("  IsComplete: ").Append(IsComplete ? "true" : "false").Append(nl)
              .Append("  Origin: ").Append(Origin ?? "<null>").Append(nl)
              .Append("  RemoteAddress: ").Append(RemoteAddress ?? "<null>").Append(nl)
              .Append("  RemotePort: ").Append(RemotePort != null ? RemotePort.ToString() : "<null>").Append(nl)
              .Append("  ContentType: ").Append(ContentType ?? "<null>").Append(nl)
              .Append("  ContentTypeCharset: ").Append(ContentTypeCharset ?? "<null>").Append(nl)
              .Append("  ContentLength(header): ").Append(ContentLength.ToString(CultureInfo.InvariantCulture)).Append(nl)
              .Append("  Content(bytes): ").Append(FormatContentPreview(Content)).Append(nl)
              .Append("  Accept-Charsets: ").Append(JoinOrEmpty(AcceptCharsets)).Append(nl)
              .Append("  Accept-Encodings: ").Append(JoinOrEmpty(AcceptEncodings)).Append(nl)
              .Append("  Accept-MediaTypes: ").Append(JoinOrEmpty(AcceptMediaTypes)).Append(nl)
              .Append("  Access-Control-Request-Method: ").Append(AccessControlRequestMethod != null ? AccessControlRequestMethod.ToString() : "<null>").Append(nl)
              .Append("  Access-Control-Request-Headers: ").Append(JoinOrEmpty(AccessControlRequestHeaders)).Append(nl)
              .Append("  Headers: ").Append(FormatHeaders(Headers)).Append(nl);

            return sb.ToString();

            string JoinOrEmpty<T>(IEnumerable<T> items) {
                if (items == null) return "<null>";

                var first = true;
                var inner = new StringBuilder();
                foreach (var item in items) {
                    if (!first) inner.Append(", ");
                    inner.Append(item != null ? item.ToString() : "<null>");
                    first = false;
                }
                return first ? "<empty>" : inner.ToString();
            }

            string FormatHeaders(IEnumerable<IHttpRequestHeader> headers) {
                if (headers == null) return "<null>";

                var list = new List<string>();
                foreach (var h in headers) {
                    if (h == null) {
                        list.Add("<null>");
                        continue;
                    }

                    string entry = null;
                    var t = h.GetType();
                    var nameProp = t.GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
                    var valueProp = t.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);

                    if (nameProp != null && valueProp != null) {
                        var name = SafeToString(nameProp.GetValue(h));
                        var value = Truncate(SafeToString(valueProp.GetValue(h)), 200);
                        entry = name + ": " + value;
                    } else {
                        entry = Truncate(h.ToString(), 200);
                    }

                    list.Add(entry);
                }

                if (list.Count == 0) return "<empty>";

                var sb2 = new StringBuilder();
                for (int i = 0; i < list.Count; i++) {
                    if (i > 0) sb2.Append("; ");
                    sb2.Append(list[i]);
                }
                return sb2.ToString();
            }

            string SafeToString(object obj) {
                return obj != null ? obj.ToString() : "<null>";
            }

            string Truncate(string s, int max) {
                if (string.IsNullOrEmpty(s)) return "<empty>";
                return s.Length <= max ? s : s.Substring(0, max) + "…";
            }

            string FormatContentPreview(byte[] data) {
                if (data == null) return "<null>";

                const int maxBytes = 32;
                var len = data.Length < maxBytes ? data.Length : maxBytes;

                var hex = new StringBuilder(len * 3);
                for (var i = 0; i < len; i++) {
                    if (i > 0) hex.Append(' ');
                    hex.Append(data[i].ToString("X2", CultureInfo.InvariantCulture));
                }

                var suffix = data.Length > maxBytes ? " …" : string.Empty;
                return data.Length.ToString(CultureInfo.InvariantCulture) + " bytes [" + hex + "]" + suffix;
            }
        }

        #region Internal Methods

        internal void AddHeader(IHttpRequestHeader header) {
            if (IsComplete) {
                throw new InvalidOperationException("Can't add header after processing request is finished!");
            }

            header.Visit(HttpRequestHandleHeaderData.Default, this);

            _headers.Add(header);
        }

        #endregion Internal Methods
    }
}