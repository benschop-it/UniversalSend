using System;
using System.Collections.Generic;
using System.Text;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.Misc {

    internal class EncodingCache : IEncodingCache {

        #region Private Fields

        private Dictionary<string, Encoding> _cache;
        private object _cacheLock;

        #endregion Private Fields

        #region Public Constructors

        public EncodingCache() {
            _cache = new Dictionary<string, Encoding>(StringComparer.OrdinalIgnoreCase);
            _cacheLock = new object();
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods
    }
}