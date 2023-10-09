using ApiNotificaciones.Model;
using Microsoft.AspNetCore.Mvc;

namespace ApiNotificaciones.Interfaces
{
    public interface IKeycloakService
    {
        //public Response ClientInfo(string token);
        public Response get_token(object cliente);
    }
}