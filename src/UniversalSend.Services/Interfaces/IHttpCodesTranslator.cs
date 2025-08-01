namespace UniversalSend.Services.Interfaces {
    public interface IHttpCodesTranslator {
        string GetHttpStatusCodeText(int statusCode);
    }
}