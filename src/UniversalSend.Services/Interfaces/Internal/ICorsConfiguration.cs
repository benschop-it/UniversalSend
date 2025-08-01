namespace UniversalSend.Services.Interfaces.Internal {

    internal interface ICorsConfiguration {

        ICorsConfiguration AddAllowedOrigin(string allowedOrigin);
    }
}