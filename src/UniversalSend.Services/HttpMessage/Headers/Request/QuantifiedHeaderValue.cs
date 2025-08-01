using System;
using System.Collections.Generic;

namespace UniversalSend.Services.HttpMessage.Headers.Request {

    internal class QuantifiedHeaderValue {

        #region Private Fields

        private static string QUALITY_KEY = "q";

        #endregion Private Fields

        #region Public Constructors

        public QuantifiedHeaderValue(string headerValue, IDictionary<string, string> quantifiers) {
            HeaderValue = headerValue;
            Quantifiers = quantifiers;

            ExtractQuality();
        }

        #endregion Public Constructors

        #region Internal Properties

        internal string HeaderValue { get; }

        internal decimal Quality { get; private set; }
        internal IDictionary<string, string> Quantifiers { get; }

        #endregion Internal Properties

        #region Internal Methods

        internal string FindQuantifierValue(string quantifierKey) {
            if (Quantifiers.ContainsKey(quantifierKey)) {
                return Quantifiers[quantifierKey];
            }

            return null;
        }

        #endregion Internal Methods

        #region Private Methods

        private void ExtractQuality() {
            if (Quantifiers.ContainsKey(QUALITY_KEY)) {
                decimal qualityAsDec;
                if (decimal.TryParse(Quantifiers[QUALITY_KEY], out qualityAsDec)) {
                    Quality = Math.Min(Math.Max(qualityAsDec, 0), 1);
                } else {
                    Quality = 0;
                }
            } else {
                Quality = 1;
            }
        }

        #endregion Private Methods
    }
}