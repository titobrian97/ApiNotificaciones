using ApiNotificaciones.Model;
using Microsoft.AspNetCore.Mvc;

namespace ApiNotificaciones.Interfaces
{
    public interface IConsumer
    {
        Task<Response> GetApiDataAsync(string url, string token, Notificacion notificacion);

    }
}