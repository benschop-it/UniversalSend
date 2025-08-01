using System;

namespace UniversalSend.Services.HttpMessage.Headers.Response {

    internal class DateHeader : HttpHeaderBase {

        #region Internal Fields

        internal static string NAME = "Date";

        #endregion Internal Fields

        #region Public Constructors

        public DateHeader(DateTime date) : base(NAME, date.ToString("r")) {
            Date = date;
        }

        #endregion Public Constructors

        #region Public Properties

        public DateTime Date { get; }

        #endregion Public Properties
    }
}