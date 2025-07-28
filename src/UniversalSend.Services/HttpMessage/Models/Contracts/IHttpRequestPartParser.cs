namespace UniversalSend.Services.HttpMessage.Models.Contracts {

    internal interface IHttpRequestPartParser {

        void HandleRequestPart(byte[] stream, MutableHttpServerRequest resultThisFar);

        byte[] UnparsedData { get; }
        bool IsFinished { get; }
        bool IsSucceeded { get; }
    }
}