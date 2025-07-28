namespace UniversalSend.Services.Webserver.Http
{
    public interface ICorsConfiguration
    {       
        ICorsConfiguration AddAllowedOrigin(string allowedOrigin);
    }
}