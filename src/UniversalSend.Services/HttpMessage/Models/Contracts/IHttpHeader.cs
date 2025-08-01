namespace UniversalSend.Services.HttpMessage.Models.Contracts {

    internal interface IHttpHeader {

        #region Public Properties

        string Name { get; }
        string Value { get; }

        #endregion Public Properties
    }
}