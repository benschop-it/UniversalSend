namespace UniversalSend.Services.HttpMessage.Models.Contracts {

    internal interface IHttpRequestHeader : IHttpHeader {

        #region Public Methods

        void Visit<T>(IHttpRequestHeaderVisitor<T> v, T arg);

        #endregion Public Methods
    }
}