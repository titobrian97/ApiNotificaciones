using ApiNotificaciones.Model;

namespace ApiNotificaciones.Interfaces
{
    public interface IRequestSender
    {
        public Response Send(HttpRequestMessage config);
        public Task<Response> SendAsync(HttpRequestMessage config);
    }
}