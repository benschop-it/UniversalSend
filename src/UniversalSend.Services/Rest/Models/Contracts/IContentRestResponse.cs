namespace UniversalSend.Services.Rest.Models.Contracts {

    public interface IContentRestResponse : IRestResponse {
        object ContentData { get; }
    }
}