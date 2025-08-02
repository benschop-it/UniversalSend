using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.HttpMessage {

    internal class HttpServerRequest : IHttpServerRequest {

        #region Public Constructors

        public HttpServerRequest(IEnumerable<IHttpRequestHeader> headers, HttpMethod? method, Uri uri,
            string httpVersion, string contentTypeCharset, IEnumerable<string> acceptCharsets, int contentLength,
            string contentType, IEnumerable<string> acceptEncodings, IEnumerable<string> acceptMediaTypes,
            byte[] content, bool isComplete, HttpMethod? accessControlRequestMethod, IEnumerable<string> accessControlRequestHeaders,
            string origin) {
            Headers = headers;
            Method = method;
            Uri = uri;
            HttpVersion = httpVersion;
            ContentTypeCharset = contentTypeCharset;
            AcceptCharsets = acceptCharsets;
            ContentLength = contentLength;
            ContentType = contentType;
            AcceptEncodings = acceptEncodings;
            AcceptMediaTypes = acceptMediaTypes;
            Content = content;
            IsComplete = isComplete;
            AccessControlRequestMethod = accessControlRequestMethod;
            AccessControlRequestHeaders = accessControlRequestHeaders;
            Origin = origin;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEnumerable<string> AcceptCharsets { get; }
        public IEnumerable<string> AcceptEncodings { get; }
        public IEnumerable<string> AcceptMediaTypes { get; }
        public IEnumerable<string> AccessControlRequestHeaders { get; }
        public HttpMethod? AccessControlRequestMethod { get; }
        public byte[] Content { get; }
        public int ContentLength { get; }
        public string ContentType { get; }
        public string ContentTypeCharset { get; }
        public IEnumerable<IHttpRequestHeader> Headers { get; }
        public string HttpVersion { get; }
        public bool IsComplete { get; }
        public HttpMethod? Method { get; }
        public string Origin { get; }
        public Uri Uri { get; }

        public override string ToString() {
            var nl = Environment.NewLine;
            var sb = new StringBuilder(512);

            sb.Append("HttpServerRequest").Append(nl)
              .Append("  Method: ").Append(Method != null ? Method.ToString() : "<null>").Append(nl)
              .Append("  Uri: ").Append(Uri != null ? Uri.ToString() : "<null>").Append(nl)
              .Append("  HttpVersion: ").Append(HttpVersion ?? "<null>").Append(nl)
              .Append("  IsComplete: ").Append(IsComplete ? "true" : "false").Append(nl)
              .Append("  Origin: ").Append(Origin ?? "<null>").Append(nl)
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
                    if (h == null) { list.Add("<null>"); continue; }

                    // Try to use Name/Value if present; else fall back to ToString()
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

        #endregion Public Properties
    }
}