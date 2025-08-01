namespace UniversalSend.Services.Interfaces.Internal {

    internal interface ICorsConfiguration {

        #region Public Methods

        ICorsConfiguration AddAllowedOrigin(string allowedOrigin);

        #endregion Public Methods
    }
}