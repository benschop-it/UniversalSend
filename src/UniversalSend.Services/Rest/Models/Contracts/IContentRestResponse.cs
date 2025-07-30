namespace UniversalSend.Services.Rest.Models.Contracts {

    internal interface IContentRestResponse : IRestResponse {
        object ContentData { get; }
    }
}