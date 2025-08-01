namespace UniversalSend.Services.Models.Schemas {

    internal class BadRequestResponse : StatusOnlyResponse {

        #region Internal Constructors

        internal BadRequestResponse() : base(400) {
        }

        #endregion Internal Constructors
    }
}