namespace UniversalSend.Services.Models.Schemas {

    internal class BadRequestResponse : StatusOnlyResponse {

        internal BadRequestResponse() : base(400) {
        }
    }
}