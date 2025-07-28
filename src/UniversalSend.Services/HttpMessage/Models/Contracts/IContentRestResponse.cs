namespace UniversalSend.Services.Models.Contracts
{
    public interface IContentRestResponse : IRestResponse
    {
        object ContentData { get; }
    }
}
