namespace UniversalSend.Services.Http {

    public interface ICorsConfiguration {

        ICorsConfiguration AddAllowedOrigin(string allowedOrigin);
    }
}