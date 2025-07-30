namespace UniversalSend.Services.Http {

    internal interface ICorsConfiguration {

        ICorsConfiguration AddAllowedOrigin(string allowedOrigin);
    }
}