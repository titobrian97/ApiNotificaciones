using ApiNotificaciones.Model;

namespace ApiNotificaciones.Interfaces
{
    public interface IBancoRepository
    {
        bool InsertarNotificacion(Notificacion notificacion, int estado, string? mensaje_error);
    }
}