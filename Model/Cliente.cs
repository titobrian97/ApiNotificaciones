using DotNetEnv;

namespace ApiNotificaciones.Model
{
    public class Cliente
    {
        public string client_id { get; } = Env.GetString("client_id");
        public string client_secret { get; } = Env.GetString("client_secret");
        /*public string username { get; } = Env.GetString("username");
        public string password { get; } = Env.GetString("password");*/
        //public string realm { get; } = Env.GetString("realm");
    }
}