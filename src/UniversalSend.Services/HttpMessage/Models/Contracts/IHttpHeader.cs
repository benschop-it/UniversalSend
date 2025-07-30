namespace UniversalSend.Services.HttpMessage.Models.Contracts {

    internal interface IHttpHeader {
        string Name { get; }
        string Value { get; }
    }
}