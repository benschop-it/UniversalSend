using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalSend.Services {

    public class EncodingCache : IEncodingCache {
        private Dictionary<string, Encoding> _cache;
        private object _cacheLock;

        public EncodingCache() {
            _cache = new Dictionary<string, Encoding>(StringComparer.OrdinalIgnoreCase);
            _cacheLock = new object();
        }

        public Encoding GetEncoding(string charset) {
            if (string.IsNullOrEmpty(charset)) {
                return null;
            }

            if (!_cache.ContainsKey(charset)) {
                lock (_cacheLock) {
                    if (!_cache.ContainsKey(charset)) {
                        Encoding charsetEncoding = null;
                        try {
                            charsetEncoding = Encoding.GetEncoding(charset);
                        } catch {
                        }

                        _cache[charset] = charsetEncoding;
                    }
                }
            }

            return _cache[charset];
        }
    }
}