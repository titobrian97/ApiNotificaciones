namespace ApiNotificaciones.Model
{
    public class ResponseModel
    {
        public ResponseModel(string id, int estado)
        {
            Id = id;
            Estado = estado;
        }

        public string Id { get; set; }
        public int Estado { get; set; }
 
    }
}
