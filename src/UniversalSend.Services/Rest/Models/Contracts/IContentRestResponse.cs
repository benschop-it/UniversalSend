namespace UniversalSend.Services.Rest.Models.Contracts {

    internal interface IContentRestResponse : IRestResponse {

        #region Public Properties

        object ContentData { get; }

        #endregion Public Properties
    }
}