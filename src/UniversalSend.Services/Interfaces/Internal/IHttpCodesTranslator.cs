namespace UniversalSend.Services.Interfaces.Internal {
    internal interface IHttpCodesTranslator {
        string GetHttpStatusCodeText(int statusCode);
    }
}