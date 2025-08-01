namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IHttpCodesTranslator {

        #region Public Methods

        string GetHttpStatusCodeText(int statusCode);

        #endregion Public Methods
    }
}