using System;
using System.Collections.Generic;
using System.Linq;
using UniversalSend.Services.Interfaces.Internal;

namespace UniversalSend.Services.Http {

    internal class ContentEncoderFactory {

        #region Internal Methods

        internal IContentEncoder GetEncoder(IEnumerable<string> acceptEncodings) {
            var firstSupportedEncoding = GetSupportedAcceptEncoding(acceptEncodings);
            switch (firstSupportedEncoding) {
                case ContentCoding.None:
                    return new NoContentEncoder();
                case ContentCoding.Deflate:
                    return new DeflateContentEncoder();
                case ContentCoding.Gzip:
                    return new GzipContentEncoder();
            }

            throw new Exception($"{firstSupportedEncoding} not supported.");
        }

        #endregion Internal Methods

        #region Private Methods

        private static ContentCoding GetAcceptEncoding(string possibleValidEncoding) {
            var trimmedEncoding = possibleValidEncoding.Trim();
            if ("gzip".Equals(trimmedEncoding, StringComparison.OrdinalIgnoreCase))
                return ContentCoding.Gzip;
            if ("deflate".Equals(trimmedEncoding, StringComparison.OrdinalIgnoreCase))
                return ContentCoding.Deflate;

            return ContentCoding.None;
        }

        private static ContentCoding GetSupportedAcceptEncoding(IEnumerable<string> acceptEncodings) {
            return acceptEncodings
                .Select(GetAcceptEncoding)
                .FirstOrDefault(x => x != ContentCoding.None);
        }

        #endregion Private Methods
    }
}